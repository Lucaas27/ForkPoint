import { createFileRoute, Link } from "@tanstack/react-router";
import { useRestaurants } from "@/features/restaurants/queries";
import {
	Card,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Input } from "@/components/ui/input";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue
} from "@/components/ui/select";
import { Plus, ChevronRight, Search } from "lucide-react";
import { useAuthContext } from "@/providers/auth-provider";
import { useState } from "react";
import { RestaurantsPager } from "@/components/restaurants-pager";

export const Route = createFileRoute("/restaurants/")({
	// Read and validate search params for pagination and filtering at route level
	validateSearch: (search: Record<string, unknown>) => {
		const page = Number(search.page) || 1;
		const size = Number(search.size) || 10;
		const searchBy = search.searchBy;
		const searchTerm = search.searchTerm;
		const categoryFilter = search.categoryFilter;

		return { page, size, searchBy, searchTerm, categoryFilter } as {
			page: number;
			size: number;
			searchBy?: string;
			searchTerm?: string;
			categoryFilter?: string;
		};
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
	const { page, size, searchBy, searchTerm, categoryFilter } = Route.useSearch();
	const { data, isLoading, error } = useRestaurants(page, size, searchBy, searchTerm, categoryFilter);
	const { hasRole } = useAuthContext();
	const canCreate = hasRole("Admin") || hasRole("Owner");
	const navigate = Route.useNavigate();

	// Initialise local state for search inputs from URL search params
	const [localSearchTerm, setLocalSearchTerm] = useState(searchTerm || "");
	const [localSearchBy, setLocalSearchBy] = useState(searchBy || "Name");
	const [localCategoryFilter, setLocalCategoryFilter] = useState(categoryFilter || "all");

	const handleSearch = () => {
		const params: { page: number; size: number; searchBy?: string; searchTerm?: string; categoryFilter?: string } = { page: 1, size };
		if (localSearchTerm.trim()) {
			params.searchBy = localSearchBy;
			params.searchTerm = localSearchTerm.trim();
		}
		if (localCategoryFilter !== "all") {
			params.categoryFilter = localCategoryFilter;
		}
		navigate({
			to: "/restaurants",
			search: params,
		});
	};

	const handleClearSearch = () => {
		setLocalSearchTerm("");
		setLocalSearchBy("Name");
		setLocalCategoryFilter("all");
		navigate({
			to: "/restaurants",
			search: { page: 1, size },
		});
	};

	const handleCategoryFilter = (value: string) => {
		setLocalCategoryFilter(value);
		const params: { page: number; size: number; searchBy?: string; searchTerm?: string; categoryFilter?: string } = { page: 1, size };
		if (localSearchTerm.trim()) {
			params.searchBy = localSearchBy;
			params.searchTerm = localSearchTerm.trim();
		}
		if (value !== "all") {
			params.categoryFilter = value;
		}
		navigate({
			to: "/restaurants",
			search: params,
		});
	};

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
	let restaurants: RestaurantItem[] = resp.items ?? [];
	const unfilteredTotalItems: number =
		resp.totalItemsCount ?? resp.total ?? restaurants.length;
	const currentPage: number = resp.pageNumber ?? page;
	const pageSize: number = resp.pageSize ?? size;

	// Apply client side category filtering
	if (categoryFilter && categoryFilter !== "all") {
		restaurants = restaurants.filter(r => r.category === categoryFilter);
	}

	// Recalculate totals after filtering
	const totalItems: number = categoryFilter && categoryFilter !== "all" ? restaurants.length : unfilteredTotalItems;
	const totalPages: number = Math.max(1, Math.ceil(totalItems / pageSize));

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
			<div className="flex flex-col sm:flex-row gap-4 items-start sm:items-center">
				<div className="flex flex-col sm:flex-row gap-2 flex-1">
					<Select value={localCategoryFilter} onValueChange={handleCategoryFilter}>
						<SelectTrigger className="w-full sm:w-40">
							<SelectValue placeholder="Filter by category" />
						</SelectTrigger>
						<SelectContent>
							<SelectItem value="all">All Categories</SelectItem>
							<SelectItem value="Italian">Italian</SelectItem>
							<SelectItem value="Chinese">Chinese</SelectItem>
							<SelectItem value="Mexican">Mexican</SelectItem>
							<SelectItem value="American">American</SelectItem>
							<SelectItem value="Japanese">Japanese</SelectItem>
							<SelectItem value="Thai">Thai</SelectItem>
							<SelectItem value="Indian">Indian</SelectItem>
							<SelectItem value="French">French</SelectItem>
							<SelectItem value="Mediterranean">Mediterranean</SelectItem>
						</SelectContent>
					</Select>
					<Select value={localSearchBy} onValueChange={setLocalSearchBy}>
						<SelectTrigger className="w-full sm:w-40">
							<SelectValue placeholder="Search by" />
						</SelectTrigger>
						<SelectContent>
							<SelectItem value="Name">Name</SelectItem>
							<SelectItem value="Description">Description</SelectItem>
						</SelectContent>
					</Select>
					<div className="flex gap-2 flex-1">
						<Input
							placeholder={`Search restaurants by ${localSearchBy.toLowerCase()}...`}
							value={localSearchTerm}
							onChange={(e) => setLocalSearchTerm(e.target.value)}
							onKeyDown={(e) => e.key === "Enter" && handleSearch()}
							className="flex-1"
						/>
						<Button onClick={handleSearch} size="sm">
							<Search className="h-4 w-4" />
						</Button>
						{(searchBy || searchTerm || categoryFilter) && (
							<Button onClick={handleClearSearch} variant="outline" size="sm">
								Clear
							</Button>
						)}
					</div>
				</div>
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
