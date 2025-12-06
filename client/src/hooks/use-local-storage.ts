import { useEffect, useState } from "react";

/**
 * Custom React hook for managing local storage.
 * @param key - The local storage key.
 * @param initialValue - The initial value if no stored value exists.
 * @returns A tuple of [value, setValue] where value is the current stored value and setValue updates it.
 */
export function useLocalStorage<T>(key: string, initialValue: T) {
	// Initialize state with value from localStorage or fallback to initialValue
	const [value, setValue] = useState<T>(() => {
		const item = localStorage.getItem(key);
		if (item === null) return initialValue;
		try {
			return JSON.parse(item) as T;
		} catch {
			// Fallback for non-JSON values (plain strings like tokens)
			return item as unknown as T;
		}
	});

	// Sync changes to localStorage whenever value or key changes
	useEffect(() => {
		if (value === undefined || value === null) {
			localStorage.removeItem(key);
		} else {
			// Store strings as-is to avoid double-quoting, JSON stringify otherwise
			if (typeof value === "string") {
				localStorage.setItem(key, value as unknown as string);
			} else {
				localStorage.setItem(key, JSON.stringify(value));
			}
		}
	}, [key, value]);

	return [value, setValue] as const;
}
