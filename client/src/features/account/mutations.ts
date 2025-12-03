import { useMutation } from '@tanstack/react-query'
import { updateMe } from '../../api/account'

// Update basic account profile fields (e.g. display name).
export function useUpdateMe() {
  return useMutation({
    mutationFn: (payload: { name: string }) => updateMe(payload),
  })
}
