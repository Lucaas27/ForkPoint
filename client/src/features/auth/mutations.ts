import { useMutation, useQueryClient } from "@tanstack/react-query";
import { register } from "../../api/auth";
import { forgotPassword, resendEmailConfirmation } from "../../api/account";
import { resetPassword, confirmEmail } from "../../api/account";
import { accountKeys } from "../account/keys";
import { useAuthContext } from "../../providers/auth-provider";
import { toast } from "sonner";
import { getErrorMessage, getSuccessMessage } from "@/lib/utils";
import { useNavigate } from "@tanstack/react-router";

export function useLogin(showToasts: boolean = true) {
	const qc = useQueryClient();
	const { login } = useAuthContext();
	const navigate = useNavigate();

	return useMutation({
		// Accept an optional returnTo in the variables so callers can pass
		// where they should be redirected after login.
		mutationFn: (payload: {
			email: string;
			password: string;
			returnTo?: string;
		}) => login({ email: payload.email, password: payload.password }),
		onSuccess: (data, variables) => {
			// Auth provider updates UI state - we need to invalidate queries.
			qc.invalidateQueries({ queryKey: accountKeys.myRestaurants });
			if (showToasts) {
				const msg = getSuccessMessage(data);
				toast.success(msg ?? "Logged in");
			}

			// Redirect or fall back to '/'.
			const url = variables?.returnTo;
			const dest = url?.startsWith("/") ? url : "/";

			navigate({ to: dest, replace: true });
		},
		onError: (err) => {
			const msg = getErrorMessage(err);
			if (showToasts) {
				toast.error(msg);
			}
		},
	});
}

export function useResetPassword(showToasts: boolean = true) {
	const navigate = useNavigate();
	return useMutation({
		mutationFn: (payload: {
			email: string;
			token: string;
			password: string;
			confirmPassword: string;
		}) => resetPassword(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg =
					getSuccessMessage(data) ??
					"Password reset successfully. You can now sign in.";
				toast.success(msg);
			}
			navigate({ to: "/login", replace: true });
		},
		onError: (err) => {
			if (showToasts) {
				const message = getErrorMessage(err);
				toast.error(message || "Failed to reset password");
			}
		},
	});
}

export function useConfirmEmail(showToasts: boolean = true) {
	return useMutation({
		mutationFn: (payload: { token: string; email: string }) =>
			confirmEmail(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg =
					getSuccessMessage(data) ?? data?.message ?? "Account verified.";
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

export function useRegister(showToasts: boolean = true) {
	const navigate = useNavigate();
	return useMutation({
		mutationFn: (payload: { email: string; password: string }) =>
			register(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg = getSuccessMessage(data);
				toast.success(msg ?? "Account created. Please verify via email.");
			}
			navigate({
				to: "/confirm",
				search: { email: data.email },
				replace: true,
			});
		},
		onError: (err) => {
			const msg = getErrorMessage(err);
			if (showToasts) {
				toast.error(msg);
			}
		},
	});
}

export function useLogout(showToasts: boolean = true) {
	const qc = useQueryClient();
	const { logOut } = useAuthContext();
	const navigate = useNavigate();

	return useMutation({
		mutationFn: () => logOut(),
		onSuccess: (data) => {
			console.log("logout success:", data);
			if (showToasts) {
				const msg = getSuccessMessage(data);
				toast.success(msg ?? "Logged out");
			}
			// invalidate user data.
			qc.invalidateQueries({ queryKey: accountKeys.myRestaurants });

			navigate({ to: "/", replace: true });
		},
		onError: (err) => {
			if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}

export function useSendPasswordReset(showToasts: boolean = true) {
	return useMutation({
		mutationFn: (payload: { email: string }) => forgotPassword(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg =
					getSuccessMessage(data) ??
					"If an account exists, we sent a reset email.";
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

export function useResendEmailConfirmation(showToasts: boolean = true) {
	return useMutation({
		mutationFn: (payload: { email: string }) =>
			resendEmailConfirmation(payload),
		onSuccess: (data) => {
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Confirmation email sent.";
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
