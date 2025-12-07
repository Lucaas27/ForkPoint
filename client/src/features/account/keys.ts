/**
 * Centralised cache/query keys for account-related data.
 * These are used with React Query to ensure consistent cache invalidation.
 */
export const accountKeys = {
	myRestaurants: ["my-restaurants"] as const,
	myProfile: ["my-profile"] as const,
};
