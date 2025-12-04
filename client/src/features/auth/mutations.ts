import { useMutation, useQueryClient } from '@tanstack/react-query'
import { login, register, logout, refreshToken } from '../../api/auth'
import { accountKeys } from '../account/keys'

// Basic auth mutations. The API layer handles token setting via setAuthToken.
export function useLogin() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (payload: { email: string; password: string }) => login(payload),
    onSuccess: () => {
      // Invalidate user-scoped data after auth state changes.
      qc.invalidateQueries({ queryKey: accountKeys.myRestaurants })
    },
  })
}

export function useRegister() {
  return useMutation({
    mutationFn: (payload: { email: string; password: string }) => register(payload),
  })
}

export function useLogout() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: () => logout(),
    onSuccess: () => {
      // Clear user-scoped data after logout.
      qc.invalidateQueries({ queryKey: accountKeys.myRestaurants })
    },
  })
}

export function useRefreshToken() {
  return useMutation({
    mutationFn: (payload: any) => refreshToken(payload),
  })
}
