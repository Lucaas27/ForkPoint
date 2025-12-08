import { useMutation, useQueryClient } from "@tanstack/react-query";
import { assignRole, removeRole } from "@/api/admin";
import { toast } from "sonner";
import { getErrorMessage, getSuccessMessage } from "@/lib/utils";

// Role management for admins.
export function useAssignRole(showToasts: boolean = true) {
	const qc = useQueryClient();

	return useMutation({
		mutationFn: (payload: { email: string; role: string }) =>
			assignRole(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Role assigned";
				toast.success(msg);
			}
			// Invalidate any users listing regardless of page/size so the UI refreshes
			qc.invalidateQueries({
				predicate: (query) =>
					Array.isArray(query.queryKey) &&
					query.queryKey[0] === "admin" &&
					query.queryKey[1] === "users",
			});

		},
		onError: (err) => {
			if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}

export function useRemoveRole(showToasts: boolean = true) {
	const qc = useQueryClient();

	return useMutation({
		mutationFn: (payload: { email: string; role: string }) =>
			removeRole(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Role removed";
				toast.success(msg);
			}
			// invalidate users lists so UI refreshes
			qc.invalidateQueries({
				predicate: (query) =>
					Array.isArray(query.queryKey) &&
					query.queryKey[0] === "admin" &&
					query.queryKey[1] === "users",
			});
		},
		onError: (err) => {
			if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}
