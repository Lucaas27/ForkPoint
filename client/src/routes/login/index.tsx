import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useLogin, useRegister } from "../../features/auth/mutations";
import { useEffect, useId, useState } from "react";
import { useAuthContext } from "../../features/auth/AuthProvider";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "../../components/ui/card";
import { Input } from "../../components/ui/input";
import { Label } from "../../components/ui/label";
import { Button } from "../../components/ui/button";
import {
	Tabs,
	TabsContent,
	TabsList,
	TabsTrigger,
} from "../../components/ui/tabs";
import { Separator } from "../../components/ui/separator";
import { LogIn, UserPlus } from "lucide-react";
import { toast } from "sonner";
import { resendEmailConfirmation, forgotPassword } from "../../api/account";

export const Route = createFileRoute("/login/")({
	component: Login,
});

function Login() {
	const navigate = useNavigate();
	const { isAuthenticated } = useAuthContext();

	// If already authenticated, redirect
	useEffect(() => {
		if (isAuthenticated) {
			navigate({ to: "/", replace: true });
		}
	}, [isAuthenticated, navigate]);

	const emailLoginId = useId();
	const passwordLoginId = useId();
	const emailRegisterId = useId();
	const passwordRegisterId = useId();
	const [email, setEmail] = useState("forkpointuser@gmail.com");
	const [password, setPassword] = useState("UserPassword1!");
	const [sendingReset, setSendingReset] = useState(false);
	const mLogin = useLogin();
	const mRegister = useRegister();

	return (
		<div className="max-w-md mx-auto">
			<Card>
				<CardHeader>
					<CardTitle className="text-2xl">Authentication</CardTitle>
					<CardDescription>Login or create a new account</CardDescription>
				</CardHeader>
				<CardContent>
					<Tabs defaultValue="login" className="w-full">
						<TabsList className="grid w-full grid-cols-2">
							<TabsTrigger value="login">Login</TabsTrigger>
							<TabsTrigger value="register">Register</TabsTrigger>
						</TabsList>

						<TabsContent value="login" className="space-y-4">
							<div className="space-y-2">
								<Label htmlFor={emailLoginId}>Email</Label>
								<Input
									id={emailLoginId}
									type="email"
									placeholder="Enter your email"
									value={email}
									onChange={(e) => setEmail(e.target.value)}
								/>
							</div>

							<div className="space-y-2">
								<Label htmlFor={passwordLoginId}>Password</Label>
								<Input
									id={passwordLoginId}
									type="password"
									placeholder="Enter your password"
									value={password}
									onChange={(e) => setPassword(e.target.value)}
								/>
								<div className="mt-1 text-right">
									<Button
										variant="link"
										className="p-0 h-auto text-sm"
										onClick={async () => {
											if (!email) {
												toast.error("Please enter your email first");
												return;
											}
											setSendingReset(true);
											try {
												const res = await forgotPassword({ email });
												const msg =
													(res as { message?: string })?.message ||
													"If an account exists, we've sent a reset email.";
												toast.success(msg);
											} catch (err) {
												const anyErr = err as {
													response?: { data?: { message?: string } };
												};
												toast.error(
													anyErr?.response?.data?.message ||
														"Failed to send reset email",
												);
											} finally {
												setSendingReset(false);
											}
										}}
										disabled={!email || sendingReset}
									>
										{sendingReset ? "Sending reset email…" : "Forgot password?"}
									</Button>
								</div>
							</div>

							<Button
								className="w-full"
								onClick={() =>
									mLogin.mutate(
										{ email, password },
										{
											onSuccess: () => {
												toast.success("Logged in");
												navigate({ to: "/", replace: true });
											},
											onError: (err: unknown) => {
												const anyErr = err as {
													response?: {
														data?: {
															title?: string;
															message?: string;
															Message?: string;
														};
													};
												};
												const msg =
													anyErr?.response?.data?.Message || "Login failed";
												toast.error(msg);
											},
										},
									)
								}
								disabled={mLogin.isPending}
							>
								<LogIn className="h-4 w-4 mr-2" />
								{mLogin.isPending ? "Logging in..." : "Login"}
							</Button>
						</TabsContent>

						<TabsContent value="register" className="space-y-4">
							<div className="space-y-2">
								<Label htmlFor={emailRegisterId}>Email</Label>
								<Input
									id={emailRegisterId}
									type="email"
									placeholder="Enter your email"
									value={email}
									onChange={(e) => setEmail(e.target.value)}
								/>
							</div>

							<div className="space-y-2">
								<Label htmlFor={passwordRegisterId}>Password</Label>
								<Input
									id={passwordRegisterId}
									type="password"
									placeholder="Create a password"
									value={password}
									onChange={(e) => setPassword(e.target.value)}
								/>
							</div>

							<Button
								className="w-full"
								onClick={() =>
									mRegister.mutate(
										{ email, password },
										{
											onSuccess: (data: unknown) => {
												const msg =
													(data as { message?: string })?.message ||
													"Account created";
												console.log("register success:", data);
												toast.success(
													msg || "Account created. Please verify via email.",
												);
												navigate({
													to: "/confirm",
													search: { email },
													replace: true,
												});
											},
											onError: (err: unknown) => {
												const anyErr = err as {
													response?: {
														data?: {
															title?: string;
															message?: string;
														};
													};
												};
												const msg =
													anyErr?.response?.data?.message ||
													"Registration failed";
												toast.error(msg);
											},
										},
									)
								}
								disabled={mRegister.isPending}
							>
								<UserPlus className="h-4 w-4 mr-2" />
								{mRegister.isPending ? "Creating account..." : "Register"}
							</Button>

							<div className="text-sm text-muted-foreground">
								<p>
									After registering, please check your email for a confirmation
									link.
								</p>
								<div className="mt-2 flex gap-2">
									<Button
										variant="link"
										size="sm"
										onClick={async () => {
											try {
												const res = await resendEmailConfirmation({ email });
												toast.success(
													res?.message || "Confirmation email sent.",
												);
											} catch (err) {
												const anyErr = err as {
													response?: { data?: { message?: string } };
												};
												toast.error(
													anyErr?.response?.data?.message ||
														"Failed to resend email.",
												);
											}
										}}
										disabled={!email || mRegister.isPending}
									>
										Resend Confirmation Email
									</Button>
								</div>
							</div>
						</TabsContent>
					</Tabs>

					<Separator className="my-6" />
				</CardContent>
			</Card>
		</div>
	);
}
