import { createFileRoute, redirect } from "@tanstack/react-router";
import {
	useCreateRestaurant,
	type CreateRestaurantPayload,
} from "../../features/restaurants/mutations";
import { useNavigate } from "@tanstack/react-router";
import { toast } from "sonner";
import { useEffect, useId, useState } from "react";
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
import { Textarea } from "../../components/ui/textarea";
import { Plus } from "lucide-react";
import { useAuthContext } from "../../features/auth/AuthProvider";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "../../components/ui/select";
import { Switch } from "../../components/ui/switch";

export const Route = createFileRoute("/restaurants/create")({
	beforeLoad: () => {
		const token = localStorage.getItem("fp_token");
		if (!token)
			throw redirect({
				to: "/login",
				search: { redirect: "/restaurants/create" },
			});
		// role check will be enforced inside component via hook
	},
	component: CreateRestaurant,
});

function CreateRestaurant() {
	const navigate = useNavigate();
	const { requireRoleRedirect } = useAuthContext();

	// Enforce Admin role via context guard
	useEffect(() => {
		const unauthorized = requireRoleRedirect("Admin", "/account", {
			redirectTo: "/restaurants/create",
		});
		if (unauthorized) {
			navigate({
				to: unauthorized.to as "/account",
				search: unauthorized.search,
				replace: true,
			});
		}
	}, [requireRoleRedirect, navigate]);
	const nameId = useId();
	const descId = useId();
	const categoryId = useId();
	const emailId = useId();
	const contactId = useId();
	const hasDeliveryId = useId();
	const streetId = useId();
	const cityId = useId();
	const countyId = useId();
	const postCodeId = useId();
	const countryId = useId();

	const [name, setName] = useState("");
	const [description, setDescription] = useState("");
	const [category, setCategory] = useState("FastFood");
	const [email, setEmail] = useState("");
	const [contactNumber, setContactNumber] = useState("");
	const [hasDelivery, setHasDelivery] = useState(false);
	const [street, setStreet] = useState("");
	const [city, setCity] = useState("");
	const [county, setCounty] = useState("");
	const [postCode, setPostCode] = useState("");
	const [country, setCountry] = useState("");
	const mutation = useCreateRestaurant();

	const categories = [
		"FastFood",
		"Chinese",
		"Indian",
		"Italian",
		"Mexican",
		"Japanese",
		"Mediterranean",
		"Burgers",
		"Bakery",
		"Cafe",
	] as const;

	return (
		<div className="max-w-2xl mx-auto">
			<Card>
				<CardHeader>
					<CardTitle className="text-2xl">Create Restaurant</CardTitle>
					<CardDescription>Add a new restaurant to the system</CardDescription>
				</CardHeader>
				<CardContent>
					<form
						className="space-y-4"
						onSubmit={(e) => {
							e.preventDefault();
							const payload: CreateRestaurantPayload = {
								name,
								description,
								category,
								hasDelivery,
								email,
								contactNumber: contactNumber || undefined,
								address: {
									street,
									city: city || undefined,
									county: county || undefined,
									postCode,
									country: country || undefined,
								},
							};
							mutation.mutate(payload, {
								onSuccess: () => {
									toast.success("Restaurant created");
									navigate({
										to: "/restaurants",
										search: { page: 1, size: 10 },
									});
								},
								onError: (err: unknown) => {
									const anyErr = err as {
										response?: {
											data?: {
												title?: string;
												message?: string;
												Message?: string;
											};
										};
									};
									const msg =
										anyErr?.response?.data?.Message ||
										"Failed to create restaurant";
									toast.error(msg);
								},
							});
						}}
					>
						<div className="space-y-2">
							<Label htmlFor={nameId}>Restaurant Name</Label>
							<Input
								id={nameId}
								placeholder="Enter restaurant name"
								value={name}
								onChange={(e) => setName(e.target.value)}
								required
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor={descId}>Description</Label>
							<Textarea
								id={descId}
								placeholder="Describe the restaurant"
								value={description}
								onChange={(e) => setDescription(e.target.value)}
								rows={4}
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor={categoryId}>Category</Label>
							<Select value={category} onValueChange={setCategory}>
								<SelectTrigger id={categoryId}>
									<SelectValue placeholder="Select a category" />
								</SelectTrigger>
								<SelectContent>
									{categories.map((c) => (
										<SelectItem key={c} value={c}>
											{c}
										</SelectItem>
									))}
								</SelectContent>
							</Select>
						</div>

						<div className="grid grid-cols-1 md:grid-cols-2 gap-4">
							<div className="space-y-2">
								<Label htmlFor={emailId}>Contact Email</Label>
								<Input
									id={emailId}
									type="email"
									placeholder="owner@example.com"
									value={email}
									onChange={(e) => setEmail(e.target.value)}
									required
								/>
							</div>
							<div className="space-y-2">
								<Label htmlFor={contactId}>Contact Number</Label>
								<Input
									id={contactId}
									placeholder="Optional"
									value={contactNumber}
									onChange={(e) => setContactNumber(e.target.value)}
								/>
							</div>
						</div>

						<div className="flex items-center justify-between py-2">
							<Label htmlFor={hasDeliveryId}>Offers Delivery</Label>
							<Switch
								id={hasDeliveryId}
								checked={hasDelivery}
								onCheckedChange={setHasDelivery}
							/>
						</div>

						<div className="space-y-2">
							<Label>Address</Label>
							<div className="space-y-3">
								<Input
									id={streetId}
									placeholder="Street"
									value={street}
									onChange={(e) => setStreet(e.target.value)}
									required
								/>
								<div className="grid grid-cols-1 md:grid-cols-2 gap-3">
									<Input
										id={cityId}
										placeholder="City"
										value={city}
										onChange={(e) => setCity(e.target.value)}
									/>
									<Input
										id={countyId}
										placeholder="County"
										value={county}
										onChange={(e) => setCounty(e.target.value)}
									/>
								</div>
								<div className="grid grid-cols-1 md:grid-cols-2 gap-3">
									<Input
										id={postCodeId}
										placeholder="Post code"
										value={postCode}
										onChange={(e) => setPostCode(e.target.value)}
										required
									/>
									<Input
										id={countryId}
										placeholder="Country"
										value={country}
										onChange={(e) => setCountry(e.target.value)}
									/>
								</div>
							</div>
						</div>

						<Button
							type="submit"
							className="w-full"
							disabled={mutation.isPending}
						>
							<Plus className="h-4 w-4 mr-2" />
							{mutation.isPending ? "Creating..." : "Create Restaurant"}
						</Button>
					</form>
				</CardContent>
			</Card>
		</div>
	);
}
