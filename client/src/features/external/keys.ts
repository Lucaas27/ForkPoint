/**
 * Centralised cache/query keys for account-related data.
 * These are used with React Query to ensure consistent cache invalidation.
 */
export const externalKeys = {
    allExternalRestaurants: (page: number, size: number) =>
        ["external-restaurants", page, size] as const,
};
