import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updateMe } from "@/api/account";
import { accountKeys } from "@/features/account/keys";
import { toast } from "sonner";
import { getErrorMessage, getSuccessMessage } from "@/lib/utils";

// Update basic account profile fields (display name).
export function useUpdateMe(showToasts: boolean = true) {
	const qc = useQueryClient();
	return useMutation({
		mutationFn: (payload: { fullName: string }) => updateMe(payload),
		onSuccess: (data) => {
			qc.invalidateQueries({ queryKey: accountKeys.myProfile });
			if (showToasts) {
				const msg = getSuccessMessage(data) ?? "Profile updated";
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
