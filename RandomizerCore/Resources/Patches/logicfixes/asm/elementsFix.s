.thumb
mov	r1,r4
add	r1,#0x6A
mov	r0,#0x40
strb	r0,[r1]
ldrb	r0,[r4,#0x0A]

ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#4]
ldrb	r2,[r0,#5]
ldr	r3,poin
loop:
ldrb	r0,[r3]
cmp	r0,#0
beq	noMatch
cmp	r0,r1
bne	next
ldrb	r0,[r3,#1]
cmp	r0,r2
beq	match
next:
add	r3,#3
b	loop

noMatch:
mov	r0,#0x40
b	end

match:
ldrb	r0,[r3,#2]
cmp	r0,#0x43
bne	end
push	{r0}
ldr	r0,=#0x2002EA4
mov	r1,#11
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800
pop	{r0}

end:
ldr	r3,=#0x809FA7B
bx	r3

.align
.ltorg
poin:
