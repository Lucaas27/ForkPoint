import { createFileRoute, Link, useNavigate } from "@tanstack/react-router";
import { useAuthContext } from "../../providers/auth-provider";
import { useEffect } from "react";
import { useMyRestaurants } from "../../features/account/queries";
import { useUpdateMe } from "../../features/account/mutations";
import { useState, useId } from "react";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "../../components/ui/card";
import { Input } from "../../components/ui/input";
import { Label } from "../../components/ui/label";
import { Button } from "../../components/ui/button";
import { Separator } from "../../components/ui/separator";
import { Badge } from "../../components/ui/badge";
import { User, Store } from "lucide-react";

export const Route = createFileRoute("/account/")({ component: Account });

type RestaurantSummary = { id: string; name: string; category?: string };

function Account() {
	const { currentUser, isAuthenticated } = useAuthContext();
	const navigate = useNavigate();
	const userRoles = currentUser?.roles ?? [];
	const hasRole = (role: string) => userRoles.some((r) => r.toLowerCase() === role.toLowerCase());

	// Redirect to login if not authenticated
	useEffect(() => {
		if (!isAuthenticated) {
			navigate({ to: "/login", search: { redirect: "/account" } });
		}
	}, [isAuthenticated, navigate]);
	const nameInputId = useId();
	const [name, setName] = useState("");

	// keep local name in sync with provider currentUser when it becomes available
	useEffect(() => {
		if (currentUser?.name) setName(currentUser.name);
	}, [currentUser]);

	const mUpdate = useUpdateMe();
	const { data } = useMyRestaurants();
	const restaurants: RestaurantSummary[] =
		(
			data as unknown as {
				ownedRestaurants?: RestaurantSummary[];
				OwnedRestaurants?: RestaurantSummary[];
			}
		)?.ownedRestaurants ??
		(
			data as unknown as {
				ownedRestaurants?: RestaurantSummary[];
				OwnedRestaurants?: RestaurantSummary[];
			}
		)?.OwnedRestaurants ??
		[];

	// Client-side pagination for owned restaurants
	const [page, setPage] = useState(1);
	const pageSize = 10;
	const totalItems = restaurants.length;
	const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));
	const canPrev = page > 1;
	const canNext = page < totalPages;
	const start = (page - 1) * pageSize;
	const end = Math.min(start + pageSize, totalItems);
	const pagedRestaurants = restaurants.slice(start, end);

	return (
		<div className="max-w-2xl mx-auto space-y-6">
			<Card>
				<CardHeader>
					<div className="flex items-center gap-2">
						<User className="h-5 w-5" />
						<CardTitle className="text-2xl">My Account</CardTitle>
					</div>
					<CardDescription>
						{(currentUser?.name ?? name)
							? `Signed in as ${currentUser?.name ?? name}`
							: "Manage your profile information"}
					</CardDescription>
				</CardHeader>
				<CardContent className="space-y-4">
					<div className="space-y-2">
						<Label htmlFor={nameInputId}>Display Name</Label>
						<Input
							id={nameInputId}
							placeholder="Enter your name"
							value={name}
							onChange={(e) => setName(e.target.value)}
						/>
					</div>

					<Button
						onClick={() => mUpdate.mutate({ fullName: name })}
						disabled={mUpdate.isPending}
					>
						{mUpdate.isPending ? "Updating..." : "Update Profile"}
					</Button>
				</CardContent>
			</Card>

			<Card>
				<CardHeader>
					<div className="flex items-center gap-2">
						<Store className="h-5 w-5" />
						<CardTitle>My Restaurants</CardTitle>
					</div>
					<CardDescription>
						{restaurants.length === 0
							? "You have no restaurants yet"
							: `Managing ${restaurants.length} restaurant${restaurants.length !== 1 ? "s" : ""}`}
					</CardDescription>
				</CardHeader>
				<CardContent>
					{restaurants.length === 0 ? (
						<div className="text-center py-8">
							{hasRole("Admin") || hasRole("Owner") ? (
								<>
									<p className="text-muted-foreground mb-4">
										Create your first restaurant to get started
									</p>
									<Link to="/restaurants/create">
										<Button>Create Restaurant</Button>
									</Link>
								</>
							) : (
								<p className="text-sm text-muted-foreground">
									Only Admins and Owners can create restaurants.
								</p>
							)}
						</div>
					) : (
						<div className="space-y-3">
							{pagedRestaurants.map((r, idx: number) => (
								<div key={r.id}>
									{idx > 0 && <Separator className="my-3" />}
									<div className="flex justify-between items-center">
										<div>
											<h4 className="font-semibold">{r.name}</h4>
											{r.category && (
												<Badge variant="secondary" className="mt-1">
													{r.category}
												</Badge>
											)}
										</div>
										<Link to="/restaurants/$id" params={{ id: r.id }}>
											<Button variant="outline" size="sm">
												View
											</Button>
										</Link>
									</div>
								</div>
							))}

							<div className="flex items-center justify-between pt-2">
								<div className="text-sm text-muted-foreground">
									Page {page} of {totalPages} • Showing {start + 1}-{end} of{" "}
									{totalItems}
								</div>
								<div className="flex items-center gap-2">
									<Button
										variant="outline"
										size="sm"
										disabled={!canPrev}
										onClick={() => setPage(1)}
									>
										First
									</Button>
									<Button
										variant="outline"
										size="sm"
										disabled={!canPrev}
										// Go to previous page by decreasing page number by 1 but not less than 1
										onClick={() => setPage((p) => Math.max(1, p - 1))}
									>
										Previous
									</Button>
									<Button
										variant="outline"
										size="sm"
										disabled={!canNext}
										// Go to next page by increasing page number by 1 but not more than totalPages
										onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
									>
										Next
									</Button>
									<Button
										variant="outline"
										size="sm"
										disabled={!canNext}
										onClick={() => setPage(totalPages)}
									>
										Last
									</Button>
								</div>
							</div>
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	);
}
