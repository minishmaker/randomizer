.thumb
push	{r4-r6}

mov	r4,#0
ldr	r6,=#0x807C4A8
@check the ammount of scrolls
mov	r5,#0x48
loop:
mov	r0,r5
mov	lr,r6
.short	0xF800
cmp	r0,#0
beq	next
add	r4,#1
next:
add	r5,#1
cmp	r5,#0x50
bne	loop

@and the ammount of upgrades
mov	r5,#0x73
loop2:
mov	r0,r5
mov	lr,r6
.short	0xF800
cmp	r0,#0
beq	next2
add	r4,#1
next2:
add	r5,#1
cmp	r5,#0x76
bne	loop2

end:
cmp	r4,#7
blo	noDice
mov	r0,#1
b	ender
noDice:
mov	r0,#0
ender:
pop	{r4-r6}
strh	r0,[r4,#0x14]
pop	{r4,pc}
