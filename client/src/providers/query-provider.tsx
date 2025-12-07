import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

export function getContext() {
	const queryClient = new QueryClient({
		defaultOptions: {
			queries: {
				// Consider data fresh for 60s. Avoids refetch on every mount.
				staleTime: 1 * 60 * 1000,
				// Don’t refetch on focus. Avoids unwanted requests when switching tabs.
				refetchOnWindowFocus: false,
				// Retry failed requests up to 2 times with a delay of 1.5s.
				retry: 2,
				retryDelay: 1.5 * 1000,
			},
		},
	});
	return {
		queryClient,
	};
}

export function Provider({
	children,
	queryClient,
}: {
	children: React.ReactNode;
	queryClient: QueryClient;
}) {
	return (
		<QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
	);
}
