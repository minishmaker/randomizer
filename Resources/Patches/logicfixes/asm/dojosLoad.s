.thumb
push	{r4-r5,lr}
mov	r4,r0
ldr	r0,=#0x80686C4
ldr	r0,[r0]
ldrb	r1,[r4,#0x0A]
add	r1,r0
ldrb	r0,[r1]
strb	r0,[r4,#0x0E]
ldrb	r5,[r4,#0x0A]
cmp	r5,#1
bne	end

@check if we have a sword
ldr	r0,=#0x2002B32
ldrh	r0,[r0]
ldr	r1,=#0x1154
and	r0,r1
cmp	r0,#0
bne	spin
mov	r0,#0xFF
b	store

@check if we have spin attack
spin:
ldr	r0,=#0x2002EA4
ldr	r1,=#12
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	rock
mov	r0,#0
b	store

@check if we have rock breaker
rock:
ldr	r0,=#0x2002EA4
ldr	r1,=#13
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	dash
@check if we have white sword or higher
ldr	r0,=#0x2002B32
ldrh	r0,[r0]
ldr	r1,=#0x1150
and	r0,r1
cmp	r0,#0
beq	dash
mov	r0,#1
b	store

@check if we have dash attack
dash:
ldr	r0,=#0x2002EA4
ldr	r1,=#14
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	thrust
@check if we have the boots
ldr	r0,=#0x2002B37
ldrb	r0,[r0]
mov	r1,#4
and	r0,r1
cmp	r0,#0
beq	end
mov	r0,#2
b	store

@check if we have down thrust
thrust:
ldr	r0,=#0x2002EA4
ldr	r1,=#15
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	end
@check if we have the cape
ldr	r0,=#0x2002B37
ldrb	r0,[r0]
mov	r1,#1
and	r0,r1
cmp	r0,#0
beq	nothing
mov	r0,#3
b	store

nothing:
mov	r0,#0xFF
b	store

store:
strb	r0,[r4,#0x0E]

end:
pop	{r4-r5,pc}
