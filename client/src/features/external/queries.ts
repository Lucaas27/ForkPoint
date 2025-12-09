import { useQuery } from "@tanstack/react-query";
import { externalKeys } from "@/features/external/keys";
import { getExternalRestaurants } from "@/api/external";

export function useExternalRestaurants(page: number = 1, size: number = 10) {
    return useQuery({
        queryKey: externalKeys.allExternalRestaurants(page, size),
        queryFn: () => getExternalRestaurants(page, size),
    });
}
