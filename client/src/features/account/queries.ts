import { useQuery } from "@tanstack/react-query";
import type { UseQueryOptions } from "@tanstack/react-query";
import { myRestaurants } from "../../api/account";
import { accountKeys } from "./keys";

// Fetch the current user's restaurants.
export function useMyRestaurants(options?: UseQueryOptions<any>) {
	return useQuery({
		queryKey: accountKeys.myRestaurants,
		queryFn: () => myRestaurants(),
		...(options as any),
	});
}
