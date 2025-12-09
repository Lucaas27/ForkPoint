import { api } from "./client";

export async function getExternalRestaurants(page: number = 1, size: number = 10) {
    const res = await api.get(`/api/external/restaurants?pageNumber=${page}&pageSize=${size}`);
    return res.data;
}
