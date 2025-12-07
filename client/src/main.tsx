import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import { RouterProvider, createRouter } from "@tanstack/react-router";

import * as TanStackQueryProvider from "./providers/query-provider.tsx";

// Import the generated route tree
import { routeTree } from "./routeTree.gen";

import "./styles.css";
import { env } from "./env";
import { Toaster } from "./components/ui/sonner";
import { AuthProvider } from "./providers/auth-provider.tsx";

// Create a new router instance

const TanStackQueryProviderContext = TanStackQueryProvider.getContext();
const router = createRouter({
	routeTree,
	context: {
		...TanStackQueryProviderContext,
	},
	defaultPreload: "intent",
	scrollRestoration: true,
	defaultStructuralSharing: true,
	defaultPreloadStaleTime: 0,
});

// Register the router instance for type safety
declare module "@tanstack/react-router" {
	interface Register {
		router: typeof router;
	}
}

// Set document title from env
if (env.VITE_APP_TITLE) {
	document.title = env.VITE_APP_TITLE;
}

// Render the app
const rootElement = document.getElementById("app");
if (rootElement && !rootElement.innerHTML) {
	const root = ReactDOM.createRoot(rootElement);
	root.render(
		<StrictMode>
			<TanStackQueryProvider.Provider {...TanStackQueryProviderContext}>
				<AuthProvider>
					<RouterProvider router={router} />
				</AuthProvider>
				<Toaster richColors position="bottom-right" />
			</TanStackQueryProvider.Provider>
		</StrictMode>,
	);
}
