import { api } from "./client";

export async function getMenuItems(restaurantId: number) {
	const res = await api.get(`/api/restaurant/${restaurantId}/menu-items`);
	return res.data;
}

export async function getMenuItemById(
	restaurantId: number,
	menuItemId: number,
) {
	const res = await api.get(
		`/api/restaurant/${restaurantId}/menu-items/${menuItemId}`,
	);
	return res.data;
}

export async function createMenuItem(restaurantId: number, payload: any) {
	const res = await api.post(
		`/api/restaurant/${restaurantId}/menu-items/create`,
		payload,
	);
	return res.data;
}

export async function deleteMenuItem(restaurantId: number, menuItemId: number) {
	const res = await api.delete(
		`/api/restaurant/${restaurantId}/menu-items/${menuItemId}`,
	);
	return res.data;
}

export async function deleteAllMenuItems(restaurantId: number) {
	const res = await api.delete(`/api/restaurant/${restaurantId}/menu-items`);
	return res.data;
}
