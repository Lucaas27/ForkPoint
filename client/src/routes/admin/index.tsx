import { createFileRoute } from '@tanstack/react-router'
import { useAssignRole, useRemoveRole } from '../../features/admin/mutations'
import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Input } from '../../components/ui/input'
import { Label } from '../../components/ui/label'
import { Button } from '../../components/ui/button'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '../../components/ui/select'
import { Shield, UserPlus, UserMinus } from 'lucide-react'

export const Route = createFileRoute('/admin/')({
    component: Admin,
})

function Admin() {
    const [userId, setUserId] = useState<number>(0)
    const [role, setRole] = useState('Owner')
    const mAssign = useAssignRole()
    const mRemove = useRemoveRole()

    return (
        <div className="max-w-2xl mx-auto">
            <Card>
                <CardHeader>
                    <div className="flex items-center gap-2">
                        <Shield className="h-5 w-5" />
                        <CardTitle className="text-2xl">Admin Panel</CardTitle>
                    </div>
                    <CardDescription>Manage user roles and permissions</CardDescription>
                </CardHeader>
                <CardContent className="space-y-6">
                    <div className="space-y-4">
                        <div className="space-y-2">
                            <Label htmlFor="userId">User ID</Label>
                            <Input
                                id="userId"
                                type="number"
                                placeholder="Enter user ID"
                                value={userId || ''}
                                onChange={(e) => setUserId(Number(e.target.value))}
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="role">Role</Label>
                            <Select value={role} onValueChange={setRole}>
                                <SelectTrigger id="role">
                                    <SelectValue placeholder="Select a role" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value="Admin">Admin</SelectItem>
                                    <SelectItem value="Owner">Owner</SelectItem>
                                    <SelectItem value="User">User</SelectItem>
                                </SelectContent>
                            </Select>
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-3">
                        <Button
                            onClick={() => mAssign.mutate({ userId, role })}
                            disabled={mAssign.isPending || !userId}
                        >
                            <UserPlus className="h-4 w-4 mr-2" />
                            {mAssign.isPending ? 'Assigning...' : 'Assign Role'}
                        </Button>

                        <Button
                            variant="destructive"
                            onClick={() => mRemove.mutate({ userId, role })}
                            disabled={mRemove.isPending || !userId}
                        >
                            <UserMinus className="h-4 w-4 mr-2" />
                            {mRemove.isPending ? 'Removing...' : 'Remove Role'}
                        </Button>
                    </div>
                </CardContent>
            </Card>
        </div>
    )
}
