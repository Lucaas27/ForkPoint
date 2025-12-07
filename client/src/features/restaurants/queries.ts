import { useQuery } from "@tanstack/react-query";
import type { UseQueryOptions } from "@tanstack/react-query";
import { getRestaurants, getRestaurantById } from "../../api/restaurants";
import { getMenuItems } from "../../api/menuItems";
import { restaurantsKeys } from "./keys";

// Fetch the restaurants collection.
export function useRestaurants(
	page: number,
	size: number,
	options?: UseQueryOptions<unknown>,
) {
	return useQuery({
		queryKey: restaurantsKeys.list(page, size),
		queryFn: () => getRestaurants({ pageNumber: page, pageSize: size }),
		...(options ?? {}),
	});
}

// Fetch a single restaurant by id.
// Disabled until id is truthy. This prevents unnecessary requests.
export function useRestaurant(id: number, options?: UseQueryOptions<any>) {
	return useQuery({
		queryKey: restaurantsKeys.detail(id),
		queryFn: () => getRestaurantById(id),
		enabled: Boolean(id),
		...(options as any),
	});
}

// Fetch menu items for a restaurant.
// Disabled until id is truthy. This prevents unnecessary requests.
export function useRestaurantMenu(id: number, options?: UseQueryOptions<any>) {
	return useQuery({
		queryKey: restaurantsKeys.menu(id),
		queryFn: () => getMenuItems(id),
		enabled: Boolean(id),
		...(options as any),
	});
}
