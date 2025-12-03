import { useMutation, useQueryClient } from '@tanstack/react-query'
import { createRestaurant, deleteRestaurant, updateRestaurant } from '../../api/restaurants'
import { deleteAllMenuItems } from '../../api/menuItems'
import { restaurantsKeys } from './keys'

// Delete a restaurant and invalidate the restaurants list so it refetches.
export function useDeleteRestaurant(id: number) {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: () => deleteRestaurant(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: restaurantsKeys.all })
    },
  })
}

// Delete all menu items for a restaurant and invalidate that restaurant's menu query.
export function useDeleteAllMenuItems(restaurantId: number) {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: () => deleteAllMenuItems(restaurantId),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: restaurantsKeys.menu(restaurantId) })
    },
  })
}

// Create a restaurant and invalidate the list so it shows up.
export function useCreateRestaurant() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (payload: { name: string; description?: string; category?: string }) => createRestaurant(payload),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: restaurantsKeys.all })
    },
  })
}

// Update a restaurant and invalidate both the list and the detail view.
export function useUpdateRestaurant(id: number) {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (payload: { name?: string; description?: string; category?: string }) => updateRestaurant(id, payload),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: restaurantsKeys.all })
      qc.invalidateQueries({ queryKey: restaurantsKeys.detail(id) })
    },
  })
}
