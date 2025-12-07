import { api } from "./client";

export async function register(payload: any) {
	const res = await api.post("/api/auth/register", payload);
	return res.data;
}
