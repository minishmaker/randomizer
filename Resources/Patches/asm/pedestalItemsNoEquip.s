.thumb
push	{r4-r5,lr}
push	{r0-r1}

@check if room is right
ldr	r0,=#0x3000BF0
ldrb	r0,[r0,#4]
cmp	r0,#0x78
bne	vanilla

@check if first flag is set
ldr	r0,=#0x2002EA4
ldr	r1,=#31
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	checksecond
ldr	r0,=#0x2002EA4
mov	r1,#31
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800
b	end

@check if second flag is set
checksecond:
ldr	r0,=#0x2002EA4
ldr	r1,=#32
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	checkthird
ldr	r0,=#0x2002EA4
mov	r1,#32
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800
b	end

@check if third flag is set
checkthird:
ldr	r0,=#0x2002EA4
ldr	r1,=#33
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	end
ldr	r0,=#0x2002EA4
mov	r1,#33
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

end:
ldr	r0,=#0x2002AF4
mov	r1,#0
strb	r1,[r0]
strb	r1,[r0,#1]
pop	{r0-r1}
pop	{r4-r5,pc}

vanilla:
pop	{r0-r1}
ldr	r0,[r1,#4]
ldr	r4,=#0xFFFF
lsr	r5,r0,#0x10
ldr	r3,=#0x807EC71
bx	r3
