import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useEffect, useState, useId } from "react";
import {
	useConfirmEmail,
	useResendEmailConfirmation,
} from "@/features/auth/mutations";
import {
	Card,
	CardHeader,
	CardTitle,
	CardContent,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

export const Route = createFileRoute("/confirm/")({
	component: ConfirmPage,
});

function ConfirmPage() {
	const navigate = useNavigate();
	const { token: qsToken, email: qsEmail } = Route.useSearch() as {
		token?: string;
		email?: string;
	};
	const emailId = useId();
	const tokenId = useId();
	const [token, setToken] = useState<string>(qsToken ?? "");
	const [email, setEmail] = useState<string>(qsEmail ?? "");

	const mConfirm = useConfirmEmail();
	const mResend = useResendEmailConfirmation();

	useEffect(() => {
		if (!qsToken || !qsEmail) return;
		mConfirm.mutate({ token: qsToken, email: qsEmail });
	}, [qsToken, qsEmail, mConfirm]);

	return (
		<div className="container mx-auto px-4 py-8 max-w-lg">
			<Card>
				<CardHeader>
					<CardTitle>Verify Your Account</CardTitle>
				</CardHeader>
				<CardContent>
					{mConfirm.isPending ? (
						<p className="text-muted-foreground">Verifying your account…</p>
					) : mConfirm.isSuccess ? (
						<div className="space-y-4">
							<p>Your account has been verified. You can now sign in.</p>
							<Button onClick={() => navigate({ to: "/login" })}>
								Go to Login
							</Button>
						</div>
					) : (
						<div className="space-y-4">
							<p className="text-muted-foreground">
								Enter your token to verify your account.
							</p>
							<div className="space-y-3">
								<div className="space-y-2">
									<Label htmlFor={emailId}>Email</Label>
									<Input
										id={emailId}
										type="email"
										placeholder="you@example.com"
										value={email}
										onChange={(e) => setEmail(e.target.value)}
									/>
								</div>
								<div className="space-y-2">
									<Label htmlFor={tokenId}>Token</Label>
									<Input
										id={tokenId}
										type="text"
										placeholder="Paste your token"
										value={token}
										onChange={(e) => setToken(e.target.value)}
									/>
								</div>
								<div className="flex flex-col sm:flex-row gap-2">
									<Button
										className="w-full sm:w-auto"
										onClick={() => mConfirm.mutate({ email, token })}
										disabled={!email || !token || mConfirm.isPending}
									>
										{mConfirm.isPending ? "Verifying..." : "Verify Account"}
									</Button>
									<Button
										variant="outline"
										className="w-full sm:w-auto"
										onClick={() => navigate({ to: "/login" })}
									>
										Back to Login
									</Button>
									{email && (
										<Button
											variant="ghost"
											className="w-full sm:w-auto"
											onClick={() => mResend.mutate({ email })}
											disabled={mResend.isPending}
										>
											{mResend.isPending
												? "Sending…"
												: "Resend Confirmation Email"}
										</Button>
									)}
								</div>
							</div>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	);
}
