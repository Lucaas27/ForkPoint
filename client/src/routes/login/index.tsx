import { createFileRoute } from "@tanstack/react-router";
import {
	useLogin,
	useRegister,
	useSendPasswordReset,
	useResendEmailConfirmation,
} from "@/features/auth/mutations";
import { useId, useState, useCallback, useEffect } from "react";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import {
	Tabs,
	TabsContent,
	TabsList,
	TabsTrigger,
} from "@/components/ui/tabs";
import { Separator } from "@/components/ui/separator";
import { LogIn, UserPlus } from "lucide-react";
import { useAuthContext } from "@/providers/auth-provider";
import { useNavigate } from "@tanstack/react-router";

export const Route = createFileRoute("/login/")({ component: Login });

function Login() {
	const emailLoginId = useId();
	const passwordLoginId = useId();
	const emailRegisterId = useId();
	const passwordRegisterId = useId();
	const [email, setEmail] = useState("forkpointadmin@gmail.com");
	const [password, setPassword] = useState("AdminPassword1!");
	const mLogin = useLogin();
	const mRegister = useRegister();
	const mSendReset = useSendPasswordReset();
	const mResendEmail = useResendEmailConfirmation();
	const navigate = useNavigate();
	const { isAuthenticated } = useAuthContext();

	const { redirect: postLoginRedirect } = Route.useSearch() as { redirect?: string };

	// Redirect authenticated users away from login page
	useEffect(() => {
		if (isAuthenticated) {
			const returnTo = postLoginRedirect?.startsWith("/") ? postLoginRedirect : "/";
			navigate({ to: returnTo, replace: true });
		}
	}, [isAuthenticated, navigate, postLoginRedirect]);


	const handleLoginSubmit = useCallback((e: React.FormEvent) => {
		e.preventDefault();
		if (!mLogin.isPending) {
			mLogin.mutate({ email, password, returnTo: postLoginRedirect })
		};

	}, [email, password, mLogin, postLoginRedirect]);

	const handleRegisterSubmit = useCallback((e: React.FormEvent) => {
		e.preventDefault();
		if (!mRegister.isPending) {
			mRegister.mutate({ email, password })
		};
	}, [email, password, mRegister]);

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
							<form onSubmit={handleLoginSubmit} className="space-y-6">
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
								</div>

								<Button
									type="submit"
									className="w-full inline-flex items-center justify-center"
									disabled={mLogin.isPending}
								>
									<LogIn className="h-4 w-4 mr-2" />
									{mLogin.isPending ? "Logging in..." : "Login"}
								</Button>
							</form>
							<div className="mt-1 text-right">
								<Button
									variant="link"
									className="p-0 h-auto text-sm"
									onClick={() => mSendReset.mutate({ email })}
									disabled={!email || mSendReset.isPending}
								>
									{mSendReset.isPending
										? "Sending reset email…"
										: "Forgot password?"}
								</Button>
							</div>
						</TabsContent>

						<TabsContent value="register" className="space-y-4">
							<form onSubmit={handleRegisterSubmit} className="space-y-6">
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
									type="submit"
									className="w-full inline-flex items-center justify-center"
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
								</div>
							</form>
							<Separator />
							<div className="mt-2 flex gap-2">
								<Button
									variant="link"
									size="sm"
									onClick={() => mResendEmail.mutate({ email })}
									disabled={
										!email || mRegister.isPending || mResendEmail.isPending
									}
								>
									{mResendEmail.isPending
										? "Sending…"
										: "Click here to resend Confirmation Email"}
								</Button>
							</div>
						</TabsContent>
					</Tabs>

					<Separator className="my-6" />
				</CardContent>
			</Card>
		</div>
	);
}
