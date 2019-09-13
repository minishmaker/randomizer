.thumb
@check if this is a kinstone
cmp	r6,#0x5C
beq	kinstone

@check if this is a dungeon item
checkKey:
cmp	r6,#0x50
blo	end
cmp	r6,#0x53
bhi	end

@check if sub id is 0
cmp	r7,#0
beq	end

@otherwise, get text id based on sub id
cmp	r7,#0x18
beq	dws
cmp	r7,#0x19
beq	cof
cmp	r7,#0x1A
beq	fow
cmp	r7,#0x1B
beq	tod
cmp	r7,#0x1C
beq	pow
cmp	r7,#0x1D
beq	dhc
cmp	r7,#0x1E
beq	rc
b	end

dws:
ldr	r0,=#0x720
b	return

cof:
ldr	r0,=#0x721
b	return

fow:
ldr	r0,=#0x722
b	return

tod:
ldr	r0,=#0x723
b	return

pow:
ldr	r0,=#0x725
b	return

dhc:
ldr	r0,=#0x727
b	return

rc:
ldr	r0,=#0x724
b	return

kinstone:
cmp	r7,#0x65
beq	tornado
cmp	r7,#0x66
beq	tornado
cmp	r7,#0x67
beq	tornado
cmp	r7,#0x68
beq	tornado
cmp	r7,#0x69
beq	tornado
cmp	r7,#0x6A
beq	totem
cmp	r7,#0x6B
beq	totem
cmp	r7,#0x6C
beq	totem
cmp	r7,#0x6D
beq	crown
b	end

tornado:
ldr	r0,=#0x71A
b	return

totem:
ldr	r0,=#0x70D
b	return

crown:
ldr	r0,=#0x717
b	return

end:
mov	r0,r8
return:
pop	{r3}
mov	r8,r3
pop	{r4-r7,pc}
