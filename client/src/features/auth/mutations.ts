import { useMutation, useQueryClient } from "@tanstack/react-query";
import { login, register, logout, refreshToken } from "../../api/auth";
import { accountKeys } from "../account/keys";
import { useAuthContext } from "./AuthProvider";

// Basic auth mutations. The API layer handles token setting via setAuthToken.
export function useLogin() {
	const qc = useQueryClient();
	const { refreshAuth } = useAuthContext();

	return useMutation({
		mutationFn: (payload: { email: string; password: string }) =>
			login(payload),
		onSuccess: () => {
			// Update React auth state and invalidate user-scoped data after auth changes.
			refreshAuth();
			qc.invalidateQueries({ queryKey: accountKeys.myRestaurants });
		},
	});
}

export function useRegister() {
	return useMutation({
		mutationFn: (payload: { email: string; password: string }) =>
			register(payload),
	});
}

export function useLogout() {
	const qc = useQueryClient();
	const { refreshAuth } = useAuthContext();

	return useMutation({
		mutationFn: () => logout(),
		onSuccess: () => {
			// Clear user-scoped data after logout and update auth state.
			refreshAuth();
			qc.invalidateQueries({ queryKey: accountKeys.myRestaurants });
		},
	});
}

export function useRefreshToken() {
	return useMutation({
		mutationFn: (payload: any) => refreshToken(payload),
	});
}
