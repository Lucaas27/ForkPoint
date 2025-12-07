import { api } from "./client";

export async function getRestaurants(params?: {
	pageNumber?: number;
	pageSize?: number;
}) {
	const { pageNumber = 1, pageSize = 10 } = params || {};
	const res = await api.get("/api/restaurants", {
		params: { pageNumber, pageSize },
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
