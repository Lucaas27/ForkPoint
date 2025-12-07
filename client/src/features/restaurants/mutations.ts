import { useMutation, useQueryClient } from "@tanstack/react-query";
import {
	createRestaurant,
	deleteRestaurant,
	updateRestaurant,
} from "@/api/restaurants";
import { deleteAllMenuItems } from "@/api/menuItems";
import { restaurantsKeys } from "./keys";
import { toast } from "sonner";
import { getErrorMessage, getSuccessMessage } from "@/lib/utils";
import { useNavigate } from "@tanstack/react-router";

// Delete a restaurant and invalidate the restaurants list so it refetches.
export function useDeleteRestaurant(id: number, showToasts: boolean = true) {
	const qc = useQueryClient();
	return useMutation({
		mutationFn: () => deleteRestaurant(id),
		onSuccess: (data) => {
			qc.invalidateQueries({ queryKey: restaurantsKeys.list(1, 10) });
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Restaurant deleted";
				toast.success(msg);
			}
		},
		onError: (err) => {
			if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}

// Delete all menu items for a restaurant and invalidate that restaurant's menu query.
export function useDeleteAllMenuItems(
	restaurantId: number,
	showToasts: boolean = true,
) {
	const qc = useQueryClient();
	return useMutation({
		mutationFn: () => deleteAllMenuItems(restaurantId),
		onSuccess: (data) => {
			qc.invalidateQueries({ queryKey: restaurantsKeys.detail(restaurantId) });
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Menu items deleted";
				toast.success(msg);
			}
		},
		onError: (err) => {
			if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}

// Create a restaurant and invalidate the list so it shows up.
export type CreateRestaurantPayload = {
	name: string;
	description: string;
	category: string;
	hasDelivery: boolean;
	email: string;
	contactNumber?: string;
	address: {
		street: string;
		city?: string;
		county?: string;
		postCode: string;
		country?: string;
	};
};

export function useCreateRestaurant(showToasts: boolean = true) {
	const qc = useQueryClient();
	const navigate = useNavigate();

	return useMutation({
		mutationFn: (payload: CreateRestaurantPayload) => createRestaurant(payload),
		onSuccess: (data) => {
			qc.invalidateQueries({ queryKey: restaurantsKeys.list(1, 10) });
			qc.invalidateQueries({
				queryKey: restaurantsKeys.detail(data.newRecordId),
			});
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Restaurant created";
				toast.success(msg);
			}

			navigate({
				to: `/restaurants/${data.newRecordId}`,
			});
		},
		onError: (err) => {
			console.log("create restaurant error:", err);
			const errResponse = (
				err as { response?: { data?: { errors: Record<string, string[]> } } }
			).response?.data?.errors;

			if (errResponse && typeof errResponse === "object") {
				const message = Object.values(errResponse).flat().join(", ");
				toast.error(message);
			} else if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}

// Update a restaurant and invalidate both the list and the detail view.
export function useUpdateRestaurant(id: number, showToasts: boolean = true) {
	const qc = useQueryClient();
	return useMutation({
		mutationFn: (payload: {
			name?: string;
			description?: string;
			category?: string;
		}) => updateRestaurant(id, payload),
		onSuccess: (data) => {
			qc.invalidateQueries({ queryKey: restaurantsKeys.detail(id) });
			qc.invalidateQueries({ queryKey: restaurantsKeys.list(1, 10) });
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Restaurant updated";
				toast.success(msg);
			}
		},
		onError: (err) => {
			if (showToasts) {
				const msg = getErrorMessage(err);
				toast.error(msg);
			}
		},
	});
}
