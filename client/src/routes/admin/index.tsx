import { createFileRoute, redirect, useNavigate } from "@tanstack/react-router";
import { toast } from "sonner";
import { useAssignRole, useRemoveRole } from "../../features/admin/mutations";
import { useEffect, useId, useState } from "react";
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
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "../../components/ui/select";
import { Shield, UserPlus, UserMinus } from "lucide-react";
import { useAuthContext } from "../../features/auth/AuthProvider";

export const Route = createFileRoute("/admin/")({
	beforeLoad: () => {
		const token = localStorage.getItem("fp_token");
		if (!token)
			throw redirect({ to: "/login", search: { redirect: "/admin" } });
	},
	component: Admin,
});

function Admin() {
	const { requireRoleRedirect } = useAuthContext();
	const navigate = useNavigate();

	useEffect(() => {
		const unauthorized = requireRoleRedirect("Admin", "/account", {
			redirectTo: "/admin",
		});
		if (unauthorized)
			navigate({
				to: unauthorized.to as "/account",
				search: unauthorized.search,
				replace: true,
			});
	}, [requireRoleRedirect, navigate]);

	const emailInputId = useId();
	const roleSelectId = useId();
	const [email, setEmail] = useState<string>("");
	const [role, setRole] = useState("Owner");
	const mAssign = useAssignRole();
	const mRemove = useRemoveRole();

	return (
		<div className="max-w-2xl mx-auto">
			<Card>
				<CardHeader>
					<div className="flex items-center gap-2">
						<Shield className="h-5 w-5" />
						<CardTitle className="text-2xl">Admin Panel</CardTitle>
					</div>
					<CardDescription>Manage user roles and permissions</CardDescription>
				</CardHeader>
				<CardContent className="space-y-6">
					<div className="space-y-4">
						<div className="space-y-2">
							<Label htmlFor={emailInputId}>User Email</Label>
							<Input
								id={emailInputId}
								type="email"
								placeholder="user@example.com"
								value={email}
								onChange={(e) => setEmail(e.target.value)}
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor={roleSelectId}>Role</Label>
							<Select value={role} onValueChange={setRole}>
								<SelectTrigger id={roleSelectId}>
									<SelectValue placeholder="Select a role" />
								</SelectTrigger>
								<SelectContent>
									<SelectItem value="Admin">Admin</SelectItem>
									<SelectItem value="Owner">Owner</SelectItem>
									<SelectItem value="User">User</SelectItem>
								</SelectContent>
							</Select>
						</div>
					</div>

					<div className="grid grid-cols-2 gap-3">
						<Button
							onClick={() =>
								mAssign.mutate(
									{ email, role },
									{
										onSuccess: () => toast.success("Role assigned"),
										onError: (err: unknown) => {
											const anyErr = err as {
												response?: {
													data?: {
														Message?: string;
													};
												};
											};
											const msg =
												anyErr?.response?.data?.Message ||
												"Failed to assign role";
											toast.error(msg);
										},
									},
								)
							}
							disabled={mAssign.isPending || !email}
						>
							<UserPlus className="h-4 w-4 mr-2" />
							{mAssign.isPending ? "Assigning..." : "Assign Role"}
						</Button>

						<Button
							variant="destructive"
							onClick={() =>
								mRemove.mutate(
									{ email, role },
									{
										onSuccess: () => toast.success("Role removed"),
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
												anyErr?.response?.data?.Message ||
												"Failed to remove role";
											toast.error(msg);
										},
									},
								)
							}
							disabled={mRemove.isPending || !email}
						>
							<UserMinus className="h-4 w-4 mr-2" />
							{mRemove.isPending ? "Removing..." : "Remove Role"}
						</Button>
					</div>
				</CardContent>
			</Card>
		</div>
	);
}
