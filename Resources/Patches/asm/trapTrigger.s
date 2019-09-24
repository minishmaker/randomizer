.thumb
ldr	r3,[r7,#0x30]
mov	r0,#0x32
ldsh	r1,[r3,r0]
ldrh	r0,[r7,#0x08]
sub	r6,r1,r0
push	{r0-r7}

@check if link is in control
ldr	r0,=#0x3003DC0
ldr	r0,[r0]
cmp	r0,#0
bne	end

@check if a trap was collected
ldr	r0,=#0x2002B38
ldrb	r1,[r0]
mov	r2,#0xC0
and	r2,r1
cmp	r2,#0
beq	end
mov	r2,#0x3F
and	r1,r2
strb	r1,[r0]

@trigger it
ldr	r1,=#0x203F1FF
ldrb	r1,[r1]
cmp	r1,#0xFF
bne	notRandom
ldr	r0,table
mov	r1,#0
randomLoop:
ldr	r2,[r0]
add	r1,#1
add	r0,#8
cmp	r2,#0
bne	randomLoop
ldr	r0,=#0x300100C
ldrb	r0,[r0]
swi	#6
notRandom:
ldr	r0,table
lsl	r1,#3
push	{r0,r1}
ldr	r0,[r0,r1]
cmp	r0,#0
beq	end
mov	lr,r0
.short	0xF800
pop	{r0,r1}
add	r0,r1
ldr	r1,[r0,#4]
mov	r0,#0
sub	r0,r1
ldr	r3,=#0x80522BC
mov	lr,r3
.short	0xF800

end:
pop	{r0-r7}
ldr	r0,=#0x80804E5
bx	r0
.align
.ltorg
table:
