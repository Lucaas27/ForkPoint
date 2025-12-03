import { useMutation } from '@tanstack/react-query'
import { assignRole, removeRole } from '../../api/admin'

// Role management for admins.
export function useAssignRole() {
  return useMutation({
    mutationFn: (payload: { userId: number; role: string }) => assignRole(payload),
  })
}

export function useRemoveRole() {
  return useMutation({
    mutationFn: (payload: { userId: number; role: string }) => removeRole(payload),
  })
}
