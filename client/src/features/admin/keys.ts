/**
 * Centralised cache/query keys for account-related data.
 * These are used with React Query to ensure consistent cache invalidation.
 */
export const usersKeys = {
    allUsers: (page: number, size: number) =>
        ["admin", "users", page, size] as const,
};
