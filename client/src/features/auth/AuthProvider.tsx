import {
	createContext,
	useCallback,
	useContext,
	useMemo,
	type ReactNode,
} from "react";
import { rolesFromToken, isExpired } from "./jwt";
import { useLocalStorage } from "../../hooks/use-local-storage";

type AuthContextValue = {
	isAuthenticated: boolean;
	hasRole: (role: string) => boolean;
	refreshAuth: () => void;
	signOut: () => void;
	requireAuthRedirect: (
		redirectTo: string,
	) => { to: string; search: Record<string, unknown> } | null;
	requireRoleRedirect: (
		role: string,
		unauthorizedTo?: string,
		opts?: { redirectTo?: string },
	) => { to: string; search: Record<string, unknown> } | null;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);
export function AuthProvider({ children }: { children: ReactNode }) {
	// Retrieve the authentication token from local storage
	const [token, setToken] = useLocalStorage<string | null>("fp_token", null);

	// Determine if the user is authenticated: token exists and is not expired
	const isAuthenticated = Boolean(token) && !isExpired(token || undefined);

	// Extract roles from the token
	const roles = rolesFromToken(token || undefined);

	// Function to check if the user has a specific role
	const hasRole = useCallback(
		(role: string) => roles.some((r) => r.toLowerCase() === role.toLowerCase()),
		[roles],
	);

	// Guard helpers to be consumed via context
	const requireAuthRedirect = useCallback(
		(redirectTo: string) => {
			if (!isAuthenticated) {
				return { to: "/login", search: { redirect: redirectTo } };
			}
			return null;
		},
		[isAuthenticated],
	);

	const requireRoleRedirect = useCallback(
		(
			role: string,
			unauthorizedTo: string = "/account",
			opts?: { redirectTo?: string },
		) => {
			if (!isAuthenticated) {
				return {
					to: "/login",
					search: { redirect: opts?.redirectTo ?? unauthorizedTo },
				};
			}
			if (!hasRole(role)) {
				return { to: unauthorizedTo, search: { unauthorized: role } };
			}
			return null;
		},
		[isAuthenticated, hasRole],
	);

	// Function to refresh authentication state from local storage
	const refreshAuth = useCallback(() => {
		try {
			const t = localStorage.getItem("fp_token");
			setToken(t ?? null);
		} catch {
			setToken(null);
		}
	}, [setToken]);

	// Sign out removes token and update state
	const signOut = useCallback(() => {
		try {
			localStorage.removeItem("fp_token");
		} catch {}
		setToken(null);
	}, [setToken]);

	// Memoize the auth context value to prevent unnecessary re-renders
	const auth = useMemo<AuthContextValue>(
		() => ({
			isAuthenticated,
			hasRole,
			refreshAuth,
			signOut,
			requireAuthRedirect,
			requireRoleRedirect,
		}),
		[
			isAuthenticated,
			hasRole,
			refreshAuth,
			signOut,
			requireAuthRedirect,
			requireRoleRedirect,
		],
	);

	// Provide the auth context to child components
	return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
}

export function useAuthContext() {
	const ctx = useContext(AuthContext);
	if (!ctx) {
		throw new Error("useAuthContext must be used within an AuthProvider");
	}
	return ctx;
}
