import { createFileRoute, Link } from "@tanstack/react-router";
import { useRestaurants } from "../../features/restaurants/queries";
import {
	Card,
	CardDescription,
	CardHeader,
	CardTitle,
} from "../../components/ui/card";
import { Button } from "../../components/ui/button";
import { Badge } from "../../components/ui/badge";
import { Skeleton } from "../../components/ui/skeleton";
import { Plus, ChevronRight } from "lucide-react";
import { useAuthContext } from "../../providers/auth-provider";

export const Route = createFileRoute("/restaurants/")({
	validateSearch: (search: Record<string, unknown>) => {
		const page = Number(search.page) || 1;
		const size = Number(search.size) || 10;
		return { page, size } as { page: number; size: number };
	},
	component: Restaurants,
});

type RestaurantItem = {
	id: number | string;
	name: string;
	description?: string;
	category?: string;
	imageUrl?: string | null;
};

type RestaurantsResponse = {
	items?: RestaurantItem[];
	restaurants?: RestaurantItem[];
	totalItemsCount?: number;
	total?: number;
	pageNumber?: number;
	pageSize?: number;
	totalPages?: number;
};

function Restaurants() {
	const { page, size } = Route.useSearch();
	const { data, isLoading, error } = useRestaurants(page, size);
	const { hasRole } = useAuthContext();
	const canCreate = hasRole("Admin") || hasRole("Owner");

	if (isLoading) {
		return (
			<div className="space-y-4">
				<Skeleton className="h-12 w-full" />
				<Skeleton className="h-32 w-full" />
				<Skeleton className="h-32 w-full" />
			</div>
		);
	}

	if (error) {
		return (
			<Card className="border-destructive">
				<CardHeader>
					<CardTitle className="text-destructive">Error</CardTitle>
					<CardDescription>Failed to load restaurants</CardDescription>
				</CardHeader>
			</Card>
		);
	}

	const resp = (data ?? {}) as unknown as RestaurantsResponse;
	const restaurants: RestaurantItem[] = resp.items ?? resp.restaurants ?? [];
	const totalItems: number =
		resp.totalItemsCount ?? resp.total ?? restaurants.length;
	const currentPage: number = resp.pageNumber ?? page;
	const pageSize: number = resp.pageSize ?? size;
	const totalPages: number =
		resp.totalPages ?? Math.max(1, Math.ceil(totalItems / pageSize));

	return (
		<div className="space-y-6">
			<div className="flex items-center justify-between">
				<div>
					<h2 className="text-3xl font-bold tracking-tight">Restaurants</h2>
					<p className="text-muted-foreground">
						Browse all available restaurants
					</p>
				</div>
				{canCreate ? (
					<Link to="/restaurants/create">
						<Button>
							<Plus className="h-4 w-4 mr-2" />
							Create Restaurant
						</Button>
					</Link>
				) : (
					<p className="text-sm text-muted-foreground">
						Only Admins and Owners can create restaurants.
					</p>
				)}
			</div>
			<div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
				{restaurants.map((r) => {
					const fallback =
						r.imageUrl ??
						`https://picsum.photos/seed/${encodeURIComponent(String(r.id))}/1200/800`;

					return (
						<Card
							key={r.id}
							className="hover:shadow-lg transition-all overflow-hidden"
						>
							<div className="aspect-video w-full bg-muted">
								<img
									src={fallback}
									alt={r.name}
									className="h-full w-full object-cover"
									loading="lazy"
								/>
							</div>
							<CardHeader>
								<div className="flex items-start justify-between">
									<div className="space-y-1">
										<CardTitle>{r.name}</CardTitle>
										<CardDescription className="line-clamp-2">
											{r.description}
										</CardDescription>
										{r.category && (
											<Badge variant="secondary">{r.category}</Badge>
										)}
									</div>
									<Link to="/restaurants/$id" params={{ id: String(r.id) }}>
										<Button variant="ghost" size="sm">
											View
											<ChevronRight className="h-4 w-4 ml-1" />
										</Button>
									</Link>
								</div>
							</CardHeader>
						</Card>
					);
				})}
			</div>
			<RestaurantsPager
				currentPage={currentPage}
				totalPages={totalPages}
				totalItems={totalItems}
				pageSize={pageSize}
			/>
		</div>
	);
}

function RestaurantsPager({
	currentPage,
	totalPages,
	totalItems,
	pageSize,
}: {
	currentPage: number;
	totalPages: number;
	totalItems: number;
	pageSize: number;
}) {
	const navigate = Route.useNavigate();

	const canPrev = currentPage > 1;
	const canNext = currentPage < totalPages;
	const start = (currentPage - 1) * pageSize + 1;
	const end = Math.min(currentPage * pageSize, totalItems);

	return (
		<div className="flex items-center justify-between pt-2">
			<div className="text-sm text-muted-foreground">
				Page {currentPage} of {totalPages} • Showing {start}-{end} of{" "}
				{totalItems}
			</div>
			<div className="flex items-center gap-2">
				<Button
					variant="outline"
					size="sm"
					disabled={!canPrev}
					onClick={() =>
						navigate({
							to: "/restaurants",
							search: { page: 1, size: pageSize },
						})
					}
				>
					First
				</Button>
				<Button
					variant="outline"
					size="sm"
					disabled={!canPrev}
					onClick={() =>
						navigate({
							to: "/restaurants",
							search: { page: currentPage - 1, size: pageSize },
						})
					}
				>
					Previous
				</Button>
				<Button
					variant="outline"
					size="sm"
					disabled={!canNext}
					onClick={() =>
						navigate({
							to: "/restaurants",
							search: { page: currentPage + 1, size: pageSize },
						})
					}
				>
					Next
				</Button>
				<Button
					variant="outline"
					size="sm"
					disabled={!canNext}
					onClick={() =>
						navigate({
							to: "/restaurants",
							search: { page: totalPages, size: pageSize },
						})
					}
				>
					Last
				</Button>
			</div>
		</div>
	);
}
