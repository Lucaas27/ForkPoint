import { useMutation } from "@tanstack/react-query";
import { assignRole, removeRole } from "../../api/admin";

// Role management for admins.
export function useAssignRole() {
	return useMutation({
		mutationFn: (payload: { email: string; role: string }) =>
			assignRole(payload),
	});
}

export function useRemoveRole() {
	return useMutation({
		mutationFn: (payload: { email: string; role: string }) =>
			removeRole(payload),
	});
}
