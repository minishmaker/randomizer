.thumb
push	{r4-r5,lr}
mov	r4,r1
mov	r5,r0

@check if swiftblade
ldrb	r1,[r5,#0x0A]
cmp	r1,#1
beq	swiftblade

@check if we have a sword
ldr	r0,=#0x2002B32
ldrh	r0,[r0]
ldr	r1,=#0x1154
and	r0,r1
cmp	r0,#0
beq	locked

@otherwise check the flag for this brother
ldr	r0,table
ldrb	r1,[r5,#0x0A]
ldrb	r1,[r0,r1]
ldr	r0,=#0x2002EA4
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	free
b	locked

locked:
mov	r0,#1
b	end

free:
mov	r0,#0
b	end

end:
str	r0,[r4,#0x14]
pop	{r4-r5,pc}

swiftblade:
@check if no valid scroll
ldrb	r0,[r5,#0x0E]
cmp	r0,#0xFF
beq	nothing
@check corresponding swiftblade scrolls
cmp	r0,#0
beq	spin
cmp	r0,#1
beq	rock
cmp	r0,#2
beq	dash
cmp	r0,#4
beq	thrust
b	free
nothing:
mov	r0,#3
strb	r0,[r5,#0x0E]
b	locked

spin:
ldr	r0,=#0x2002EA4
ldr	r1,=#12
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	free
b	nothing

rock:
ldr	r0,=#0x2002EA4
ldr	r1,=#13
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	free
b	nothing

dash:
ldr	r0,=#0x2002EA4
ldr	r1,=#14
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	free
b	nothing

thrust:
ldr	r0,=#0x2002EA4
ldr	r1,=#15
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	free
b	nothing

.align
.ltorg
table:
