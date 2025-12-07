import { api } from "./client";

export async function getRestaurants(params?: {
	pageNumber?: number;
	pageSize?: number;
	searchBy?: string;
	searchTerm?: string;
	categoryFilter?: string;
}) {
	const {
		pageNumber = 1,
		pageSize = 10,
		searchBy,
		searchTerm,
		categoryFilter,
	} = params || {};
	const queryParams: Record<string, string | number> = { pageNumber, pageSize };
	if (searchBy) queryParams.searchBy = searchBy;
	if (searchTerm) queryParams.searchTerm = searchTerm;
	if (categoryFilter) queryParams.categoryFilter = categoryFilter;
	const res = await api.get("/api/restaurants", {
		params: queryParams,
	});
	return res.data;
}

export async function getRestaurantById(id: number) {
	const res = await api.get(`/api/restaurants/${id}`);
	return res.data;
}

export async function createRestaurant(payload: unknown) {
	const res = await api.post("/api/restaurants/create", payload);
	return res.data;
}

export async function updateRestaurant(id: number, payload: unknown) {
	const res = await api.patch(`/api/restaurants/${id}`, payload);
	return res.data;
}

export async function deleteRestaurant(id: number) {
	const res = await api.delete(`/api/restaurants/${id}`);
	return res.data;
}
