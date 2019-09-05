.thumb
ldr	r3,table
ldr	r2,=#0x3000BF0
ldrb	r0,[r2,#4]
ldrb	r1,[r2,#5]
loop:
ldrh	r2,[r3]
cmp	r2,#0
beq	end
cmp	r2,r0
bne	next
ldrh	r2,[r3,#2]
cmp	r2,r1
beq	match
next:
add	r3,#12
b	loop

match:
ldr	r0,[r3,#4]
ldr	r1,[r3,#8]
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

@get the item
ldrb	r0,[r4,#0x0A]
ldrb	r1,[r4,#0x0B]
mov	r2,#0

end:
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800
ldr	r3,=#0x805E208
mov	lr,r3
.short	0xF800
pop	{r4,pc}

.align
.ltorg
table:
