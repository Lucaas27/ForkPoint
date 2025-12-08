import { api } from "./client";

export type AdminUser = {
	id: number;
	email: string;
	name?: string | null;
	roles?: string[];
};

export type GetUsersResponse = {
	items?: AdminUser[];
	totalItemsCount?: number;
	pageNumber?: number;
	pageSize?: number;
	totalPages?: number;
};

export async function assignRole(payload: Record<string, unknown>) {
	const res = await api.post("/api/admin/users/roles", payload);
	return res.data;
}

export async function removeRole(payload: Record<string, unknown>) {
	const res = await api.delete("/api/admin/users/roles", { data: payload });
	return res.data;
}

export async function getUsers(page: number = 1, size: number = 10) {
	const res = await api.get<GetUsersResponse>(
		`/api/admin/users?pageNumber=${page}&pageSize=${size}`,
	);
	return res.data;
}
