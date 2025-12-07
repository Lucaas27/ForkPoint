import {
	createContext,
	useCallback,
	useContext,
	useMemo,
	useState,
	useEffect,
	useLayoutEffect,
	type PropsWithChildren,
} from "react";
import { api } from "@/api/client";
import type { InternalAxiosRequestConfig } from "axios";

type User = {
	id: string;
	email: string;
	roles: string[];
	name: string;
};

type AuthContext = {
	accessToken?: string | null;
	currentUser?: User | null;
	isAuthenticated: boolean;
	logOut: () => Promise<void>;
	login: (payload: { email: string; password: string }) => Promise<void>;
	hasRole: (role: string) => boolean;
};

type AuthProviderProps = PropsWithChildren;

const TOKEN_KEY = "fp_accessToken";

const AuthContext = createContext<AuthContext | undefined>(undefined);

export function useAuthContext() {
	const ctx = useContext(AuthContext);
	if (!ctx) {
		throw new Error("useAuthContext must be used within an AuthProvider");
	}
	return ctx;
}

export function AuthProvider({ children }: AuthProviderProps) {
	// Load saved access token from local storage on startup
	const [accessToken, setAccessToken] = useState<string | null>(() => {
		try {
			return localStorage.getItem(TOKEN_KEY);
		} catch {
			return null;
		}
	});
	// Store user info (fetched fresh each time)
	const [currentUser, setCurrentUser] = useState<User | null>(null);
	// True when we have both token and user data
	const isAuthenticated = Boolean(accessToken && currentUser);

	// Save token to local storage whenever it changes
	useEffect(() => {
		if (accessToken) {
			localStorage.setItem(TOKEN_KEY, accessToken);
		} else {
			localStorage.removeItem(TOKEN_KEY);
		}
	}, [accessToken]);

	// When token changes, fetch fresh user details from server
	useEffect(() => {
		if (!accessToken) {
			setCurrentUser(null);
			return;
		}
		const fetchUser = async () => {
			try {
				const res = await api.get("/api/account/me");
				setCurrentUser(res.data.user ?? res.data ?? null);
			} catch {
				setCurrentUser(null);
			}
		};
		fetchUser();
	}, [accessToken]);

	// Login function - sends credentials to server and saves the returned token
	const login = useCallback(async (payload: { email: string; password: string }) => {
		try {
			const res = await api.post("/api/auth/login", payload);
			const token = res.data?.accessToken ?? null;
			setAccessToken(token);
		} catch (err) {
			setAccessToken(null);
			throw err;
		}
	}, []);

	// Logout function - clears local state and tells server to invalidate session
	const logOut = useCallback(async () => {
		setAccessToken(null);
		setCurrentUser(null);
		await api.post("/api/auth/logout");
	}, []);

	// Automatically add auth header to outgoing requests (runs before useEffect)
	useLayoutEffect(() => {
		const interceptor = api.interceptors.request.use((config: InternalAxiosRequestConfig & { _retry?: boolean }) => {
			if (accessToken && !config._retry) {
				config.headers = config.headers || {};
				config.headers.Authorization = `Bearer ${accessToken}`;
			}
			return config;
		});
		return () => api.interceptors.request.eject(interceptor);
	}, [accessToken]);

	// Handle expired tokens by automatically refreshing them
	// Use useLayoutEffect so the interceptor is installed before any
	// access-token dependent requests (like the initial /me fetch) run.
	useLayoutEffect(() => {
		const interceptor = api.interceptors.response.use(
			(response) => response,
			async (error) => {
				const request = error.config as (InternalAxiosRequestConfig & { _retry?: boolean }) | undefined;
				if (!request || request._retry || (error.response?.status !== 401 && error.response?.status !== 403)) {
					return Promise.reject(error);
				}
				// Skip refresh for auth endpoints
				if (request.url?.includes("/api/auth/")) {
					return Promise.reject(error);
				}

				try {
					const res = await api.post("/api/auth/refresh-token");
					const token = res.data?.accessToken ?? null;
					if (token) {
						setAccessToken(token);
						request.headers = request.headers || {};
						request.headers.Authorization = `Bearer ${token}`;
						request._retry = true;
						return api(request);
					}
				} catch {
					setAccessToken(null);
					setCurrentUser(null);
				}
				return Promise.reject(error);
			}
		);
		return () => api.interceptors.response.eject(interceptor);
	}, []);

	// Helper function to check if user has a specific role
	const hasRole = useCallback((role: string) => {
		return Boolean(currentUser?.roles?.some(r => r.toLowerCase() === role.toLowerCase()));
	}, [currentUser]);

	// Create the auth context object with all the functions and data
	const auth = useMemo<AuthContext>(
		() => ({ accessToken, isAuthenticated, logOut, login, currentUser, hasRole }),
		[accessToken, isAuthenticated, logOut, login, currentUser, hasRole]
	);

	// Provide auth context to all child components
	return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
}
