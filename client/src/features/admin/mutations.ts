import { useMutation } from "@tanstack/react-query";
import { assignRole, removeRole } from "@/api/admin";
import { toast } from "sonner";
import { getErrorMessage, getSuccessMessage } from "@/lib/utils";

// Role management for admins.
export function useAssignRole(showToasts: boolean = true) {
	return useMutation({
		mutationFn: (payload: { email: string; role: string }) =>
			assignRole(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Role assigned";
				toast.success(msg);
			}
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
	return useMutation({
		mutationFn: (payload: { email: string; role: string }) =>
			removeRole(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Role removed";
				toast.success(msg);
			}
		},
		onError: (err) => {
			if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}
