import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useId, useState } from "react";
import {
	Card,
	CardHeader,
	CardTitle,
	CardContent,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { useResetPassword } from "@/features/auth/mutations";

export const Route = createFileRoute("/reset-password/")({
	component: ResetPasswordPage,
});

function ResetPasswordPage() {
	const navigate = useNavigate();
	const { token: qsToken, email: qsEmail } = Route.useSearch() as {
		token?: string;
		email?: string;
	};

	const emailId = useId();
	const tokenId = useId();
	const passwordId = useId();
	const confirmId = useId();

	const [email, setEmail] = useState(qsEmail ?? "");
	const [token, setToken] = useState(qsToken ?? "");
	const [password, setPassword] = useState("");
	const [confirm, setConfirm] = useState("");
	const resetMutation = useResetPassword();

	return (
		<div className="container mx-auto px-4 py-8 max-w-lg">
			<Card>
				<CardHeader>
					<CardTitle>Reset Password</CardTitle>
				</CardHeader>
				<CardContent className="space-y-4">
					<div className="space-y-2">
						<Label htmlFor={emailId}>Email</Label>
						<Input
							id={emailId}
							type="email"
							value={email}
							onChange={(e) => setEmail(e.target.value)}
							placeholder="you@example.com"
						/>
					</div>
					<div className="space-y-2">
						<Label htmlFor={tokenId}>Token</Label>
						<Input
							id={tokenId}
							type="text"
							value={token}
							onChange={(e) => setToken(e.target.value)}
							placeholder="Paste your token"
						/>
					</div>
					<div className="space-y-2">
						<Label htmlFor={passwordId}>New Password</Label>
						<Input
							id={passwordId}
							type="password"
							value={password}
							onChange={(e) => setPassword(e.target.value)}
							placeholder="Enter new password"
						/>
					</div>
					<div className="space-y-2">
						<Label htmlFor={confirmId}>Confirm Password</Label>
						<Input
							id={confirmId}
							type="password"
							value={confirm}
							onChange={(e) => setConfirm(e.target.value)}
							placeholder="Confirm new password"
						/>
					</div>
					<div className="flex flex-col sm:flex-row gap-2">
						<Button
							className="w-full sm:w-auto"
							onClick={() =>
								resetMutation.mutate({
									email,
									token,
									password,
									confirmPassword: confirm,
								})
							}
							disabled={resetMutation.isPending}
						>
							{resetMutation.isPending ? "Submitting..." : "Reset Password"}
						</Button>
						<Button
							variant="outline"
							className="w-full sm:w-auto"
							onClick={() => navigate({ to: "/login" })}
						>
							Back to Login
						</Button>
					</div>
				</CardContent>
			</Card>
		</div>
	);
}
