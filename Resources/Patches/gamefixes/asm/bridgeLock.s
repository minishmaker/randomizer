.thumb
push	{r4,lr}
@check if we have a bomb bag
ldr	r0,=#0x2002AC0
add	r0,#0x2E
ldrb	r0,[r0]
cmp	r0,#0
bne	end

@check if we have spin attack and at least blue sword
ldr	r0,=#0x2002B32
ldrb	r1,[r0,#0x12]
mov	r2,#1
and	r1,r2
cmp	r1,#0
beq	set
ldrh	r0,[r0]
ldr	r1,=#0x1100
and	r0,r1
cmp	r0,#0
bne	end

@unset the bridge flag
set:
ldr	r0,=#0x2002CCD
ldrb	r1,[r0]
mov	r2,#0xEF
and	r1,r2
strb	r1,[r0]

end:
ldr	r3,=#0x804F130
mov	lr,r3
.short	0xF800
mov	r0,#0x89
ldr	r3,=#0x804F149
bx	r3
