import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useMutation } from "@tanstack/react-query";
import { useId, useState } from "react";
import {
	Card,
	CardHeader,
	CardTitle,
	CardContent,
} from "../../components/ui/card";
import { Input } from "../../components/ui/input";
import { Label } from "../../components/ui/label";
import { Button } from "../../components/ui/button";
import { resetPassword } from "../../api/account";
import { toast } from "sonner";

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
	const resetMutation = useMutation({
		mutationFn: () =>
			resetPassword({ email, token, password, confirmPassword: confirm }),
		onSuccess: () => {
			toast.success("Password reset successfully. You can now sign in.");
			navigate({ to: "/login", replace: true });
		},
		onError: (err: unknown) => {
			const anyErr = err as {
				response?: {
					data?: { errors?: Record<string, string[]>; message?: string };
				};
			};
			const errors = anyErr?.response?.data?.errors;
			const message = anyErr?.response?.data?.message;
			if (errors) {
				const flat = Object.values(errors).flat();
				toast.error(flat[0] ?? "Request failed");
			} else {
				toast.error(message || "Failed to reset password");
			}
		},
	});

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
							onClick={() => resetMutation.mutate()}
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
