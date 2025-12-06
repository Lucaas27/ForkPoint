import { api } from "./client";

export async function updateMe(payload: Record<string, unknown>) {
	const res = await api.patch("/api/account/update", payload);
	return res.data;
}

export async function adminUpdateUser(
	userId: number,
	payload: Record<string, unknown>,
) {
	const res = await api.patch(`/api/account/update/${userId}`, payload);
	return res.data;
}

export async function forgotPassword(payload: { email: string }) {
	const res = await api.post("/api/account/forgot-password", payload);
	return res.data;
}

export async function resetPassword(payload: {
	email: string;
	token: string;
	password: string;
}) {
	const res = await api.post("/api/account/reset-password", payload);
	return res.data;
}

export async function confirmEmail(params: { token: string; email: string }) {
	const res = await api.get("/api/account/verify", { params });
	return res.data as { isSuccess?: boolean; message?: string };
}

export async function resendEmailConfirmation(payload: { email: string }) {
	const res = await api.post("/api/account/resend-email-confirmation", payload);
	return res.data as { isSuccess?: boolean; message?: string };
}

export async function myRestaurants() {
	const res = await api.get("/api/account/restaurants");
	return res.data;
}
