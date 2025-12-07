import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
}

// Helper to extract error message from API errors
export function getErrorMessage(err: unknown) {
	const anyErr = err as {
		response?: { data?: { message?: string; Message?: string } };
	};
	return (
		anyErr?.response?.data?.message ??
		anyErr?.response?.data?.Message ??
		"Request failed"
	);
}

// Helper to extract message from API responses
export function getSuccessMessage(data: unknown) {
	const anyData = data as { message?: string; Message?: string };
	return anyData?.message ?? anyData?.Message ?? null;
}
