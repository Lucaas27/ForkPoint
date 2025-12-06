import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updateMe } from "../../api/account";
import { accountKeys } from "@/features/account/keys";

// Update basic account profile fields (display name).
export function useUpdateMe() {
	const qc = useQueryClient();
	return useMutation({
		mutationFn: (payload: { fullName: string }) => updateMe(payload),
		onSuccess: () => {
			qc.invalidateQueries({ queryKey: accountKeys.myProfile });
		},
	});
}
