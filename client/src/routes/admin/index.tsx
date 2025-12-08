import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useAssignRole, useRemoveRole } from "@/features/admin/mutations";
import { useState, useEffect } from "react";
import { useAuthContext } from "@/providers/auth-provider";
import { useUsers } from "@/features/admin/queries";
import type { AdminUser } from "@/api/admin";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Shield } from "lucide-react";
import PaginationControl from "@/components/pagination-control";

export const Route = createFileRoute("/admin/")({
	validateSearch: (search: Record<string, unknown>) => {
		const page = Number(search.page) || 1;
		const size = Number(search.size) || 10;
		return { page, size } as { page: number; size: number };
	},
	component: Admin,
});

function Admin() {
	const { page = 1, size = 10 } = Route.useSearch();
	const pageNumber = Number(page) || 1;
	const pageSize = Number(size) || 10;

	const { data: usersResp, isLoading: usersLoading, error: usersError } = useUsers(pageNumber, pageSize);
	const users = (usersResp?.items ?? []) as AdminUser[];
	const totalItems = usersResp?.totalItemsCount ?? users.length;
	const totalPages = Math.max(1, Math.ceil((totalItems ?? 0) / pageSize));

	const [selectedRoles, setSelectedRoles] = useState<Record<number | string, string>>({});

	const mAssign = useAssignRole();
	const mRemove = useRemoveRole();
	const navigate = useNavigate();
	const { isAuthenticated, hasRole } = useAuthContext();

	useEffect(() => {
		if (!isAuthenticated) {
			navigate({ to: "/login" });
		}
	}, [isAuthenticated, navigate]);

	if (!hasRole("Admin")) {
		return (
			<div className="p-4">
				<Card>
					<CardHeader>
						<CardTitle>
							<div className="flex items-center gap-2">
								<Shield size={18} />
								Admin
							</div>
						</CardTitle>
						<CardDescription>Admin area</CardDescription>
					</CardHeader>
					<CardContent>
						<div className="text-sm text-muted-foreground">You are not authorized to view this page.</div>
					</CardContent>
				</Card>
			</div>
		);
	}

	return (
		<div className="p-4">
			<Card>
				<CardHeader>
					<CardTitle>
						<div className="flex items-center gap-2">
							<Shield size={18} />
							Admin — Users
						</div>
					</CardTitle>
					<CardDescription>Manage users and roles</CardDescription>
				</CardHeader>
				<CardContent>
					{usersLoading ? (
						<div>Loading users...</div>
					) : usersError ? (
						<div className="text-destructive">Failed to load users</div>
					) : (
						<div className="space-y-4">
							<div className="overflow-x-auto">
								<table className="w-full table-auto border-collapse">
									<thead>
										<tr className="text-left bg-muted">
											<th className="py-3 px-4 text-sm font-medium">Email</th>
											<th className="py-3 px-4 text-sm font-medium">Name</th>
											<th className="py-3 px-4 text-sm font-medium">Roles</th>
											<th className="py-3 px-4 text-sm font-medium">Actions</th>
										</tr>
									</thead>
									<tbody>
										{users.map((u) => (
											<tr key={u.id} className="border-t">
												<td className="py-3 px-4 text-sm align-top">{u.email}</td>
												<td className="py-3 px-4 text-sm align-top">{u.name ?? "-"}</td>
												<td className="py-3 px-4 text-sm align-top">
													{(u.roles ?? []).map((r) => (
														<span key={r} className="inline-flex items-center mr-3 mb-2 px-3 py-1 rounded bg-muted text-sm">
															{r}
															<Button type="button"
																variant={"ghost"}
																className="ml-2 text-xs text-destructive" onClick={() => mRemove.mutate({ email: u.email, role: r })}>
																✕
															</Button>
														</span>
													))}
												</td>
												<td className="py-3 px-4 text-sm align-top">
													<div className="flex items-center">
														<Select value={selectedRoles[u.id] ?? "Owner"} onValueChange={(v) => setSelectedRoles((prev) => ({ ...prev, [u.id]: v }))}>
															<SelectTrigger>
																<SelectValue placeholder="Role" />
															</SelectTrigger>
															<SelectContent>
																<SelectItem value="Admin">Admin</SelectItem>
																<SelectItem value="Owner">Owner</SelectItem>
																<SelectItem value="User">User</SelectItem>
															</SelectContent>
														</Select>
														<Button size="sm" className="ml-3" onClick={() => mAssign.mutate({ email: u.email, role: selectedRoles[u.id] ?? "Owner" })}>
															Assign
														</Button>
													</div>
												</td>
											</tr>
										))}
									</tbody>
								</table>
							</div>

							<div className="flex items-center justify-between">
								<div>Showing {users.length} users</div>
								<div>
									<PaginationControl
										currentPage={pageNumber}
										totalPages={totalPages}
										onPageChange={(p) => navigate({ to: "/admin", search: { page: p, size: pageSize } })}
									/>
								</div>
							</div>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	);
}
