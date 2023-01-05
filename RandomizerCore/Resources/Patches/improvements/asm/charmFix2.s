.thumb
ldr	r0, [sp]
ldrb	r1, [r0, #0x1A]
cmp	r1, #0
beq	vanilla
ldr	r1, =#0x3001160
cmp	r0, r1
bne	vanilla

@load the palette to the already existing one
mov	r0, r6
mov	r1, r5
ldr	r3, =#0x801D300
mov	lr, r3
.short	0xF800
ldr	r0, [sp]

vanilla:
mov	r1, r5
ldr	r3, =#0x801D1D4
mov	lr, r3
.short	0xF800

end:
ldr	r3, =#0x801D128
mov	lr, r3
.short	0xF800