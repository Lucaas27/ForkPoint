import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useState } from "react";

import { useRestaurant } from "@/features/restaurants/queries";
import { useAuthContext } from "@/providers/auth-provider";
import {
	useDeleteAllMenuItems,
	useDeleteRestaurant,
} from "@/features/restaurants/mutations";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import {
	Sheet,
	SheetTrigger,
	SheetContent,
	SheetHeader,
	SheetFooter,
	SheetTitle,
	SheetDescription,
	SheetClose,
} from "@/components/ui/sheet";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Switch } from "@/components/ui/switch";
import { Badge } from "@/components/ui/badge";
import { Separator } from "@/components/ui/separator";
import {
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
	AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Trash2, UtensilsCrossed } from "lucide-react";
import { useCreateMenuItem } from "@/features/restaurants/mutations";

export const Route = createFileRoute("/restaurants/$id")({
	component: RestaurantDetail,
});

type MenuItem = {
	id: string | number;
	name: string;
	description?: string;
	price: number;
	imageUrl?: string | null;
	isVegetarian?: boolean;
	isVegan?: boolean;
	kiloCalories?: number;
};

type Restaurant = {
	id: number;
	name: string;
	description?: string;
	category: string;
	hasDelivery?: boolean;
	email?: string;
	contactNumber?: string;
	address?: unknown;
	menuItems?: MenuItem[];
	ownedByCurrentUser?: boolean;
};

function RestaurantDetail() {
	const { id: idParam } = Route.useParams();
	const id = Number(idParam);
	const { isAuthenticated } = useAuthContext();
	const [open, setOpen] = useState(false);
	const [name, setName] = useState("");
	const [description, setDescription] = useState("");
	const [price, setPrice] = useState<number | undefined>(undefined);
	const [kiloCalories, setKiloCalories] = useState<number | undefined>(undefined);
	const [isVegetarian, setIsVegetarian] = useState(false);
	const [isVegan, setIsVegan] = useState(false);
	const createMenu = useCreateMenuItem(id);
	const creating = Boolean((createMenu as unknown as { isLoading?: boolean }).isLoading);

	// Fetch and cache the restaurant by a stable key.
	// React Query handles loading/error state and caching
	// de-dupes requests, and can refetch in the background.
	const { data } = useRestaurant(id);

	// Delete the restaurant, then invalidate the restaurants list so it refetches.
	const delRestaurant = useDeleteRestaurant(id);

	// Delete all menu items, then invalidate this restaurant’s menu cache.
	const delAllMenu = useDeleteAllMenuItems(id);

	const restaurant = (data as unknown as { restaurant?: Restaurant })
		?.restaurant;

	const isOwner = restaurant?.ownedByCurrentUser ?? false;

	const menuItems: MenuItem[] = restaurant?.menuItems ?? [];

	const navigate = useNavigate();

	return (
		<div className="space-y-6">
			<Card>
				<CardHeader>
					<div className="flex items-start justify-between gap-3">
						<div className="space-y-1">
							<CardTitle className="text-3xl">{restaurant?.name}</CardTitle>
							<CardDescription>{restaurant?.description}</CardDescription>
							{restaurant?.category && (
								<Badge className="mt-2">{restaurant?.category}</Badge>
							)}
						</div>
						{isAuthenticated && isOwner && (
							<div className="flex flex-col sm:flex-row gap-2 sm:gap-3 sm:flex-wrap">
								<AlertDialog>
									<AlertDialogTrigger asChild>
										<Button
											variant="outline"
											size="sm"
											className="w-full sm:w-auto"
										>
											<Trash2 className="h-4 w-4 mr-2" />
											Delete All Menu
										</Button>
									</AlertDialogTrigger>
									<AlertDialogContent>
										<AlertDialogHeader>
											<AlertDialogTitle>
												Delete all menu items?
											</AlertDialogTitle>
											<AlertDialogDescription>
												This will permanently delete all menu items for this
												restaurant.
											</AlertDialogDescription>
										</AlertDialogHeader>
										<AlertDialogFooter>
											<AlertDialogCancel>Cancel</AlertDialogCancel>
											<AlertDialogAction
												onClick={() => delAllMenu.mutate()}
												disabled={!id || Number.isNaN(id)}
											>
												Delete
											</AlertDialogAction>
										</AlertDialogFooter>
									</AlertDialogContent>
								</AlertDialog>

								<AlertDialog>
									<AlertDialogTrigger asChild>
										<Button
											variant="destructive"
											size="sm"
											className="w-full sm:w-auto"
										>
											<Trash2 className="h-4 w-4 mr-2" />
											Delete Restaurant
										</Button>
									</AlertDialogTrigger>
									<AlertDialogContent>
										<AlertDialogHeader>
											<AlertDialogTitle>Delete restaurant?</AlertDialogTitle>
											<AlertDialogDescription>
												This action cannot be undone. This will permanently
												delete the restaurant and all associated data.
											</AlertDialogDescription>
										</AlertDialogHeader>
										<AlertDialogFooter>
											<AlertDialogCancel>Cancel</AlertDialogCancel>
											<AlertDialogAction
												onClick={() => {
													delRestaurant.mutate()
													navigate({ to: "/restaurants", replace: true, search: { page: 1, size: 10 } });
												}}
												disabled={!id || Number.isNaN(id)}
											>
												Delete
											</AlertDialogAction>
										</AlertDialogFooter>
									</AlertDialogContent>
								</AlertDialog>
							</div>
						)}
					</div>
				</CardHeader>
			</Card>

			<Card>
				<CardHeader>
					<div className="flex items-center justify-between gap-2">
						<div className="flex items-center gap-2">
							<UtensilsCrossed className="h-5 w-5" />
							<CardTitle>Menu Items</CardTitle>
						</div>
						{isAuthenticated && isOwner && (
							<Sheet open={open} onOpenChange={setOpen}>
								<SheetTrigger asChild>
									<Button size="sm">Create Menu Item</Button>
								</SheetTrigger>

								<SheetContent side="right">
									<SheetHeader>
										<SheetTitle>Create Menu Item</SheetTitle>
										<SheetDescription>
											Add a new dish to this restaurant's menu.
										</SheetDescription>
									</SheetHeader>

									<div className="p-4 space-y-3">
										<div className="flex flex-col gap-2">
											<Label>Name</Label>
											<Input value={name} onChange={(e) => setName(e.target.value)} />
										</div>

										<div className="flex flex-col gap-2">
											<Label>Description</Label>
											<Textarea value={description} onChange={(e) => setDescription(e.target.value)} />
										</div>

										<div className="grid grid-cols-2 gap-2">
											<div className="flex flex-col gap-2">
												<Label>Price (£)</Label>
												<Input type="number" value={price ?? ""} onChange={(e) => setPrice(e.target.value === "" ? undefined : parseFloat(e.target.value))} />
											</div>
											<div className="flex flex-col gap-2">
												<Label>Calories</Label>
												<Input type="number" value={kiloCalories ?? ""} onChange={(e) => setKiloCalories(e.target.value === "" ? undefined : parseInt(e.target.value, 10))} />
											</div>
										</div>

										<div className="flex items-center gap-4">
											<div className="flex items-center gap-2">
												<Switch checked={isVegetarian} onCheckedChange={(v) => setIsVegetarian(Boolean(v))} />
												<span className="text-sm">Vegetarian</span>
											</div>
											<div className="flex items-center gap-2">
												<Switch checked={isVegan} onCheckedChange={(v) => setIsVegan(Boolean(v))} />
												<span className="text-sm">Vegan</span>
											</div>
										</div>
									</div>

									<SheetFooter>
										<div className="flex items-center justify-end gap-2 w-full">
											<SheetClose asChild>
												<Button variant="ghost">Cancel</Button>
											</SheetClose>
											<Button
												onClick={() => {
													const payload: Record<string, unknown> = {
														name,
														description,
														price,
														kiloCalories,
														isVegetarian,
														isVegan,
													};
													createMenu.mutate(payload, {
														onSuccess: () => {
															// reset form and close sheet
															setName("");
															setDescription("");
															setPrice(undefined);
															setKiloCalories(undefined);
															setIsVegetarian(false);
															setIsVegan(false);
															setOpen(false);
														},
													});
												}}
												disabled={creating}
											>
												{creating ? "Creating..." : "Create"}
											</Button>
										</div>
									</SheetFooter>
								</SheetContent>
							</Sheet>
						)}
					</div>
				</CardHeader>
				<CardContent>
					{menuItems.length === 0 ? (
						<p className="text-muted-foreground text-center py-8">
							No menu items yet
						</p>
					) : (
						<div className="space-y-4">
							{menuItems.map((m: MenuItem, idx: number) => (
								<div key={m.id}>
									{idx > 0 && <Separator className="my-4" />}
									<div className="flex justify-between items-start gap-4">
										<div className="flex-1">
											<h4 className="font-semibold">{m.name}</h4>
											{m.description && (
												<p className="text-sm text-muted-foreground mt-1">
													{m.description}
												</p>
											)}
											<div className="mt-2 flex flex-wrap items-center gap-2 text-xs text-muted-foreground">
												<span>{m.kiloCalories} kcal</span>
												{m.isVegetarian && (
													<Badge variant="outline">Vegetarian</Badge>
												)}
												{m.isVegan && <Badge variant="outline">Vegan</Badge>}
											</div>
										</div>
										<div className="flex items-start gap-3">
											{m.imageUrl && (
												<img
													src={m.imageUrl}
													alt={m.name}
													className="h-16 w-16 rounded object-cover border"
												/>
											)}
											<Badge variant="secondary" className="self-start">
												£{m.price.toFixed(2)}
											</Badge>
										</div>
									</div>
								</div>
							))}
						</div>
					)}
				</CardContent>
			</Card>
		</div>
	);
}
