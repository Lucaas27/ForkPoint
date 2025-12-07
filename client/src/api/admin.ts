import { api } from "./client";

export async function assignRole(payload: Record<string, unknown>) {
	const res = await api.post("/api/admin/users/roles", payload);
	return res.data;
}

export async function removeRole(payload: Record<string, unknown>) {
	const res = await api.delete("/api/admin/users/roles", { data: payload });
	return res.data;
}
