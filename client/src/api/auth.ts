import { api } from "./client";

export async function register(payload: Record<string, unknown>) {
	const res = await api.post("/api/auth/register", payload);
	return res.data;
}
